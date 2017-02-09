using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X_Wing_Visual_Builder.Model
{
    enum UpgradeType { All = 1, Elite = 2, Astromech = 3, Torpedo = 4, Missile = 5, Cannon = 6, Turret = 7, Bomb = 8, Crew = 9,
                       SalvagedAstromech = 10, System = 11, Title = 12, Modification = 13, Illicit = 14, Cargo = 15, Hardpoint = 16,
                       Team = 17, Tech = 18 }
    enum Faction { All = 1, Rebel = 2, Imperial = 3, Scum = 4 }
    enum ShipSize { All = 1, Small = 2, Large = 3, Huge = 4 }
    enum Ship { All = 1,
                A_Wing = 2,
                ARC_170 = 3,
                Attack_Shuttle = 4,
                B_Wing = 5,
                CR90 = 6,
                E_Wing = 7,
                Firespray_31 = 8,
                G_1A = 9,
                G24 = 10,
                Gozanti_Class_Cruiser = 11,
                GR_75_ = 12,
                HWK_290 = 13,
                IG_2000 = 14,
                Imperial_Raider = 15,
                Inquisitors_TIE = 16,
                Jumpmaster_5000 = 17,
                K_Wing = 18,
                Kihraxz_Fighter = 19,
                Lambda_Shuttle = 20,
                M3_A_Interceptor = 21,
                Protectorate_Starfighter = 22,
                Shadow_Caster = 23,
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
                VT_49 = 36,
                X_Wing = 37,
                Y_Wing = 38,
                YT_1300 = 39,
                YT_2400 = 40,
                YV_666 = 41,
                Z_95 = 42,
                Aggressor = 43,
                C_ROC = 44,
                Lancer_Class_Pursuit_Craft = 45,
                Raider_Class_Corvette = 46,
                TIE_Advanced_Prototype = 47,
                TIE_Striker = 48,
                U_Wing = 49,
                Quadjumper = 50
    }

    enum Action { BarrelRoll, Boost, Cloak, Coordinate, Evade, Focus, Jam, Recover, Reinforce, RotateArc, SLAM, TargetLock }
    enum Maneuver { White, Green, Red }
}
