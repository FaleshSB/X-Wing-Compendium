using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X_Wing_Visual_Builder.Model
{
    public enum Maneuver { LSloop = 1, RSloop = 2, KTurn = 3, LTurn = 4, RTurn = 5, Stop = 6, LBank = 7, RBank = 8, Streight = 9, LTalon = 10, RTalon = 11, LBack = 12, RBack = 13, Back = 14 }
    public enum ManeuverDificulty { White = 1, Green = 2, Red = 3}
    public enum DieResult { Success = 1, Failure = 2, UsedFail = 3 }
    public enum PageName { BrowseCards = 1, UpgradeQuiz = 2, PilotQuiz = 3, SquadsPage = 4, CalculateStats = 5 }
    public enum UpgradeType { Elite = 2, Astromech = 3, Torpedo = 4, Missile = 5, Cannon = 6, Turret = 7, Bomb = 8, Crew = 9,
                       SalvagedAstromech = 10, System = 11, Title = 12, Modification = 13, Illicit = 14, Cargo = 15, Hardpoint = 16,
                       Team = 17, Tech = 18 }
    public enum UpgradeSort { Name = 1, Cost = 2 }
    public enum Faction { All = 1, Rebel = 2, Imperial = 3, Scum = 4 }
    public enum ShipSize { All = 1, Small = 2, Large = 3, Huge = 4 }
    public enum ShipType {
                All = 1,
                A_Wing = 2,
                ARC_170 = 3,
                Attack_Shuttle = 4,
                B_Wing = 5,
                CR90_Corvette = 6,
                E_Wing = 7,
                Firespray_31 = 8,
                G_1A_Starfighter = 9,
                Gozanti_Class_Cruiser = 11,
                GR_75_Medium_Transport = 12,
                HWK_290 = 13,
                JumpMaster_5000 = 17,
                K_Wing = 18,
                Kihraxz_Fighter = 19,
                Lambda_Class_Shuttle = 20,
                M3_A_Interceptor = 21,
                Protectorate_Starfighter = 22,
                StarViper = 24,
                T_70_X_Wing = 25,
                TIE_Advanced = 26,
                TIE_Bomber = 27,
                TIE_Defender = 28,
                TIE_Fighter = 29,
                TIE_Interceptor = 30,
                TIE_Phantom = 31,
                TIE_Punisher = 32,
                TIE_FO_Fighter = 33,
                TIE_SF_Fighter = 34,
                VCX_100 = 35,
                VT_49_Decimator = 36,
                X_Wing = 37,
                Y_Wing = 38,
                YT_1300 = 39,
                YT_2400 = 40,
                YV_666 = 41,
                Z_95_Headhunter = 42,
                Aggressor = 43,
                C_ROC_Cruiser = 44,
                Lancer_Class_Pursuit_Craft = 45,
                Raider_Class_Corvette = 46,
                TIE_Advanced_Prototype = 47,
                TIE_Striker = 48,
                U_Wing = 49,
                Quadjumper = 50,
                Rebel_TIE_Fighter = 51,
                Upsilon_Class_Shuttle = 52
    }

    public enum Action { BarrelRoll = 1, Boost = 2, Cloak = 3, Coordinate = 4, Evade = 5, Focus = 6, Jam = 7, Recover = 8, Reinforce = 9, RotateArc = 10, SLAM = 11, TargetLock = 12 }
    public enum RollType { Attack, Defend }
    public enum DieFace { Blank, Focus, Evade, Hit, Crit }
}
