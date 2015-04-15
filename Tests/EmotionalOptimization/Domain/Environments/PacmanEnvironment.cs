// ------------------------------------------
// PacmanEnvironment.cs, Learning.Tests.EmotionalOptimization
//
// Created by Pedro Sequeira, 2012/10/10
//
// pedro.sequeira@gaips.inesc-id.pt
// ------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using Learning.Domain.Actions;
using Learning.Domain.Agents;
using Learning.Domain.Cells;
using Learning.Domain.Environments;
using Learning.Domain.States;
using Learning.Tests.EmotionalOptimization.Domain.Ghosts;
using Learning.Tests.EmotionalOptimization.Testing;

namespace Learning.Tests.EmotionalOptimization.Domain.Environments
{
    [Serializable]
    public class PacmanEnvironment : SingleAgentEnvironment
    {
        public const string GHOST_ID = "ghost";
        public const string DOT_ID = "dot";
        public const string BIG_DOT_ID = "big-dot";
        public const string FINAL_GHOST_ID = "final-ghost";
        public const string FINAL_DOT_ID = "final-dot";
        public const string WEAK_GHOST_ID = "weak-ghost";
        public const string DEADLY_GHOST_ID = "deadly-ghost";

        private const string WEAKENED_GHOST_IMG_PATH = "../../../../bin/resources/pacman/ghost_weak.png";
        private const string GHOST2_IMG_PATH = "../../../../bin/resources/pacman/ghost_blue.png";
        private const string GHOST1_IMG_PATH = "../../../../bin/resources/pacman/ghost_red.png";
        private const string BIG_DOT_IMG_PATH = "../../../../bin/resources/pacman/big-dot.png";
        private const int GHOST_CROSS_DOOR_PROB = 5; //in 10

        private const double DOT_REWARD_DEFAULT = 0.1f;
        private const double BIG_DOT_REWARD_DEFAULT = 0.8f;
        private const double DEATH_REWARD_DEFAULT = -1f;
        private const double FINAL_GHOST_WEAK_REWARD = 1f;
        private const int MAX_LIVES_DEFAULT = 3;
        private const int MAX_STEPS_WEAK = 20;
        private const double TASK_REWARD = 1f;

        protected readonly Dictionary<Cell, ICellElement> dotCells = new Dictionary<Cell, ICellElement>();
        protected readonly Dictionary<Ghost, Cell> previousGhostsCell = new Dictionary<Ghost, Cell>();
        protected bool agentLostLife;
        protected Cell doorLeft;
        protected Cell doorRight;
        protected ICellElement finalDot;
        protected int numDotsEaten;
        protected int numGhostsEaten;
        protected int numStepsWeak;
        protected int numTotalDots;
        protected Cell previousAgentCell;

        public PacmanEnvironment() : base(0, 0)
        {
        }

        public new IPacmanScenario Scenario
        {
            get { return base.Scenario as IPacmanScenario; }
        }

        protected SmartGhost SmartGhost { get; private set; }

        protected KeeperGhost KeeperGhost { get; private set; }

        protected CellElement BigDot { get; private set; }

        public uint NumLives { get; set; }

        public override void Init()
        {
            base.Init();

            this.NumLives = this.Scenario.MaxLives;

            //creates ghost elements
            this.SmartGhost = new SmartGhost
                                  {
                                      IdToken = GHOST_ID,
                                      Description = GHOST_ID,
                                      Visible = true,
                                      Walkable = true,
                                      HasSmell = true,
                                      ImagePath = GHOST1_IMG_PATH,
                                      WeakenedGhostImagePath = WEAKENED_GHOST_IMG_PATH,
                                      State = GhostState.Normal,
                                      Color = System.Drawing.Color.FromArgb(255, 0, 0)
                                  };

            this.KeeperGhost = new KeeperGhost(this.SmartGhost)
                                   {
                                       IdToken = GHOST_ID,
                                       Description = GHOST_ID,
                                       Visible = true,
                                       Walkable = true,
                                       HasSmell = true,
                                       ImagePath = GHOST2_IMG_PATH,
                                       WeakenedGhostImagePath = WEAKENED_GHOST_IMG_PATH,
                                       State = GhostState.Normal,
                                       Color = System.Drawing.Color.FromArgb(0, 0, 255)
                                   };

            //creates big dot element
            this.BigDot = new CellElement
                              {
                                  IdToken = BIG_DOT_ID,
                                  Description = BIG_DOT_ID,
                                  Visible = true,
                                  Walkable = true,
                                  HasSmell = true,
                                  ImagePath = BIG_DOT_IMG_PATH,
                                  Color = System.Drawing.Color.FromArgb(255, 255, 102)
                              };

            this.InitDots();
            this.InitDoors();
        }

        public override void Reset()
        {
            //resets variables
            this.numDotsEaten = 0;
            this.numStepsWeak = 0;
            this.numGhostsEaten = 0;
            this.NumLives = this.Scenario.MaxLives;

            if (this.Agent == null) return;

            if (this.Agent.Cell == null)
            {
                this.Agent.Environment = this;
                this.SmartGhost.Agent = this.Agent;
                this.Init();
            }

            //resets elements
            this.ResetPositions();
            this.ResetDots();
            this.ResetGhosts();

            this.previousAgentCell = null;
            this.previousGhostsCell.Clear();
        }

        public override void Dispose()
        {
            base.Dispose();
            this.dotCells.Clear();
            this.previousGhostsCell.Clear();
            this.BigDot.Dispose();
            this.SmartGhost.Dispose();
            this.KeeperGhost.Dispose();
        }

        public override void Update()
        {
            //updates dynamics
            this.CheckGhost();
            this.UpdateGhost(this.SmartGhost);
            this.UpdateGhost(this.KeeperGhost);
            this.CheckEatenDot();
            this.CheckMagicDoor();

            //stores positions of elements
            this.previousAgentCell = this.Agent.Cell;
            this.previousGhostsCell[this.SmartGhost] = this.SmartGhost.Cell;
            this.previousGhostsCell[this.KeeperGhost] = this.KeeperGhost.Cell;

            base.Update();

            this.agentLostLife = false;
        }

        public override bool AgentFinishedTask(IAgent agent, IState state, IAction action)
        {
            // task ends when 
            // - ghosts' weak phase ends or
            // - the agent eats all dots or
            // - the agent gets eaten by ghosts or
            // - the agent eats all the ghosts
            return
                (this.Scenario.PowerPelletEnabled && (this.Scenario.MaxLives == 0) &&
                 (this.numStepsWeak >= MAX_STEPS_WEAK)) ||
                ((state != null) && (state is IStimuliState) &&
                 (((IStimuliState) state).Sensations.Contains(FINAL_DOT_ID) ||
                  ((this.Scenario.MaxLives == 0) && ((IStimuliState) state).Sensations.Contains(GHOST_ID)) ||
                  ((IStimuliState) state).Sensations.Contains(DEADLY_GHOST_ID) ||
                  (this.Scenario.PowerPelletEnabled && ((IStimuliState) state).Sensations.Contains(FINAL_GHOST_ID))));
        }

        public override double GetAgentReward(IAgent agent, IState state, IAction action)
        {
            //combination of dots and ghosts rewards
            return this.GetGhostReward(agent, state, action) + this.GetDotReward(agent, state, action);
        }

        protected virtual void CheckMagicDoor()
        {
            if (this.previousAgentCell == null) return;

            //checks magic door moves
            if ((previousAgentCell == this.doorLeft) && this.Agent.ShortTermMemory.CurrentAction is MoveLeft)
                this.Agent.Cell = this.doorRight;
            else if ((previousAgentCell == this.doorRight) && this.Agent.ShortTermMemory.CurrentAction is MoveRight)
                this.Agent.Cell = this.doorLeft;
        }

        protected virtual void CheckEatenDot()
        {
            if (this.previousAgentCell == null) return;

            //checks big dot eaten
            if (!this.Scenario.HideBigDot && this.BigDot.Visible && (this.previousAgentCell == this.BigDot.Cell))
            {
                //hides dot and ghosts become weakened
                this.numDotsEaten++;
                this.BigDot.Visible = false;
                this.BigDot.ForceRepaint = true;
                this.PutGhostsWeak();
            }

            //checks eaten dot
            if (!this.Scenario.HideDots)
            {
                var dotElement = this.dotCells[previousAgentCell];
                if (dotElement.Visible)
                {
                    //hide dot and increases dot eaten counter
                    this.numDotsEaten++;
                    dotElement.Visible = false;
                    dotElement.ForceRepaint = true;
                }

                //tries to identify the final dot
                this.IdentifyFinalDot();
            }
        }

        protected void IdentifyFinalDot()
        {
            //checks for final dot
            if (this.numDotsEaten != this.numTotalDots - 1) return;

            //changes remaining dot id to final
            foreach (var dotElement in this.dotCells.Values.Where(dotElement => dotElement.Visible))
            {
                //found the visible (and final) dot, mark it
                this.finalDot = dotElement;
                this.finalDot.IdToken = FINAL_DOT_ID;
                break;
            }
        }

        protected virtual void ResetGhosts()
        {
            this.SmartGhost.State = this.KeeperGhost.State = GhostState.Normal;
            this.SmartGhost.Visible = this.KeeperGhost.Visible = true;
        }

        protected void PutGhostsWeak()
        {
            if (!this.Scenario.PowerPelletEnabled) return;

            this.SmartGhost.State = this.KeeperGhost.State = GhostState.Weak;
            this.SmartGhost.IdToken = this.KeeperGhost.IdToken = WEAK_GHOST_ID;
            if (this.Scenario.HideKeeperGhost) this.SmartGhost.IdToken = FINAL_GHOST_ID;
        }

        protected virtual void UpdateGhost(Ghost ghost)
        {
            //only update movement if agent did not just lost a life
            if (this.agentLostLife) return;

            //checks if ghosts are able to cross magic door
            if (this.CheckMagicDoor(ghost)) return;

            //update ghosts normally
            ghost.Update();
        }

        protected virtual void CheckGhost()
        {
            //checks if the agent has been eaten
            this.CheckGhostAteAgent();

            //checks if ghosts were eaten while weak
            this.CheckAgentAteGhost();
        }

        protected virtual void CheckGhostAteAgent()
        {
            //checks whether there was a deadly encounter between ghost and agent (agent lost a life)
            if ((this.KeeperGhost.State == GhostState.Weak) || (this.Scenario.MaxLives == 0) ||
                (this.previousAgentCell == null) || (this.previousGhostsCell.Count == 0) ||
                (!this.previousAgentCell.Equals(this.previousGhostsCell[this.SmartGhost]) &&
                 !this.previousAgentCell.Equals(this.previousGhostsCell[this.KeeperGhost])))
                return;

            //if agent was eaten by ghost, decrease lives and resets positions (everything else maintains intact)
            this.NumLives--;
            this.ResetPositions();
            this.agentLostLife = true;
        }

        protected void CheckAgentAteGhost()
        {
            //checks whether ghosts can be eaten atm
            if (!this.Scenario.PowerPelletEnabled || (this.KeeperGhost.State != GhostState.Weak)) return;

            // increment weak state counter and verifies ghosts return to normal state
            this.numStepsWeak++;
            if (this.numStepsWeak > MAX_STEPS_WEAK)
                this.ResetGhosts();

            //checks whether there was an encounter between ghost and agent
            if ((this.previousAgentCell == null) || (this.previousGhostsCell.Count == 0) ||
                ((this.previousAgentCell != this.previousGhostsCell[this.SmartGhost]) &&
                 (this.previousAgentCell != this.previousGhostsCell[this.KeeperGhost])))
                return;

            //increment ghost eaten counter
            this.numGhostsEaten++;

            //sets the other ghost as final and hides the ghost
            if (this.previousAgentCell == this.previousGhostsCell[this.SmartGhost])
            {
                this.KeeperGhost.IdToken = FINAL_GHOST_ID;
                this.SmartGhost.Visible = false;
            }
            else
            {
                this.SmartGhost.IdToken = FINAL_GHOST_ID;
                this.KeeperGhost.Visible = false;
            }
        }

        protected virtual bool CheckMagicDoor(Ghost ghost)
        {
            var prob = this.rand.Next(10);
            if (prob >= GHOST_CROSS_DOOR_PROB)
                if (ghost.Cell == this.doorRight)
                {
                    ghost.Cell = this.doorLeft;
                    return true;
                }
                else if (ghost.Cell == this.doorLeft)
                {
                    ghost.Cell = this.doorRight;
                    return true;
                }
            return false;
        }

        protected virtual void ResetPositions()
        {
            //agent starts from top-middle position, ghosts from middle position
            this.Agent.Cell = this.Cells[3, 1];
            this.SmartGhost.Cell = this.Cells[3, 3];
            this.KeeperGhost.Cell = this.Scenario.HideKeeperGhost ? null : this.SmartGhost.Cell;

            //if only one life, ghosts are deadly from beggining
            this.SmartGhost.IdToken = this.KeeperGhost.IdToken = (this.NumLives == 1) ? DEADLY_GHOST_ID : GHOST_ID;
        }

        protected virtual void ResetDots()
        {
            //puts all dots to visible
            foreach (var dotElement in this.dotCells.Values)
                dotElement.Visible = dotElement.ForceRepaint = true;

            //resets final dot (if exists)
            if (this.finalDot != null)
                this.finalDot.IdToken = this.finalDot == this.BigDot ? BIG_DOT_ID : DOT_ID;
            this.finalDot = null;
        }

        protected virtual void InitDoors()
        {
            this.doorLeft = this.Cells[0, 3];
            this.doorRight = this.Cells[6, 3];
        }

        protected virtual void InitDots()
        {
            //collects references to all dots or removes them
            foreach (var cell in this.Cells)
                if (!cell.ContainsElement(DOT_ID))
                    continue;
                else if (this.Scenario.HideDots)
                    cell.Elements.RemoveWhere(element => element.IdToken == DOT_ID);
                else
                    this.dotCells.Add(cell, cell.GetElement(DOT_ID));

            this.numTotalDots = this.dotCells.Count;

            this.InitBigDot();
        }

        protected virtual void InitBigDot()
        {
            if (this.Scenario.HideBigDot) return;

            //replaces dot in the middle with big dot
            var bigDotCell = this.Cells[3, 3];
            bigDotCell.Elements.RemoveWhere(element => element.IdToken == DOT_ID);
            this.BigDot.Cell = bigDotCell;
            this.dotCells[bigDotCell] = this.BigDot;
        }

        protected virtual double GetDotReward(IAgent agent, IState state, IAction action)
        {
            if ((state == null) || !(state is IStimuliState)) return 0;
            var sensations = ((IStimuliState) state).Sensations;

            //gives dot reward according to present sensation
            return sensations.Contains(FINAL_DOT_ID)
                       ? TASK_REWARD
                       : sensations.Contains(DOT_ID)
                             ? this.Scenario.DotReward
                             : sensations.Contains(BIG_DOT_ID) ? this.Scenario.BigDotReward : 0;
        }

        protected virtual double GetGhostReward(IAgent agent, IState state, IAction action)
        {
            //if agent eats ghost, positive reward, if ghost eats agent, negative reward
            if ((state == null) || !(state is IStimuliState)) return 0;
            var sensations = ((IStimuliState) state).Sensations;

            return (((this.Scenario.MaxLives != 0) && sensations.Contains(DEADLY_GHOST_ID)) ||
                    ((this.Scenario.MaxLives == 0) && sensations.Contains(GHOST_ID)))
                       ? this.Scenario.DeathReward
                       : (this.Scenario.PowerPelletEnabled && sensations.Contains(FINAL_GHOST_ID))
                             ? FINAL_GHOST_WEAK_REWARD
                             : 0;
        }
    }
}