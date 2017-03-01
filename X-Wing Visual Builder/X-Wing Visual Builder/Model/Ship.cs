using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X_Wing_Visual_Builder.Model
{
    public class Ship
    {
        public ShipType shipType { get; set; }
        public ShipSize shipSize { get; set; }
        public string name { get; set; }

        public Ship(ShipType shipType)
        {
            this.shipType = shipType;

            switch (shipType)
            {
                case ShipType.A_Wing:
                    name = "A-Wing";
                    break;
                case ShipType.ARC_170:
                    name = "ARC-170";
                    break;
                case ShipType.Attack_Shuttle:
                    name = "Attack Shuttle";
                    break;
                case ShipType.B_Wing:
                    name = "B-Wing";
                    break;
                case ShipType.CR90_Corvette:
                    name = "CR90 Corvette";
                    break;
                case ShipType.E_Wing:
                    name = "E-Wing";
                    break;
                case ShipType.Firespray_31:
                    name = "Firespray-31";
                    break;
                case ShipType.G_1A_Starfighter:
                    name = "G-1A Starfighter";
                    break;
                case ShipType.Gozanti_Class_Cruiser:
                    name = "Gozanti-Class Cruiser";
                    break;
                case ShipType.GR_75_Medium_Transport:
                    name = "GR-75 Medium Transport";
                    break;
                case ShipType.HWK_290:
                    name = "HWK-290";
                    break;
                case ShipType.JumpMaster_5000:
                    name = "JumpMaster 5000";
                    break;
                case ShipType.K_Wing:
                    name = "K-Wing";
                    break;
                case ShipType.Kihraxz_Fighter:
                    name = "Kihraxz Fighter";
                    break;
                case ShipType.Lambda_Class_Shuttle:
                    name = "Lambda-Class Shuttle";
                    break;
                case ShipType.M3_A_Interceptor:
                    name = "M3-A Interceptor";
                    break;
                case ShipType.Protectorate_Starfighter:
                    name = "Protectorate Starfighter";
                    break;
                case ShipType.StarViper:
                    name = "StarViper";
                    break;
                case ShipType.T_70_X_Wing:
                    name = "T-70 X-wing";
                    break;
                case ShipType.TIE_Advanced:
                    name = "TIE Advanced";
                    break;
                case ShipType.TIE_Bomber:
                    name = "TIE Bomber";
                    break;
                case ShipType.TIE_Defender:
                    name = "TIE Defender";
                    break;
                case ShipType.TIE_Fighter:
                    name = "TIE Fighter";
                    break;
                case ShipType.TIE_Interceptor:
                    name = "TIE Interceptor";
                    break;
                case ShipType.TIE_Phantom:
                    name = "TIE Phantom";
                    break;
                case ShipType.TIE_Punisher:
                    name = "TIE Punisher";
                    break;
                case ShipType.TIE_FO_Fighter:
                    name = "TIE/fo Fighter";
                    break;
                case ShipType.TIE_SF_Fighter:
                    name = "TIE/sf Fighter";
                    break;
                case ShipType.VCX_100:
                    name = "VCX-100";
                    break;
                case ShipType.VT_49_Decimator:
                    name = "VT-49 Decimator";
                    break;
                case ShipType.X_Wing:
                    name = "X-Wing";
                    break;
                case ShipType.Y_Wing:
                    name = "Y-Wing";
                    break;
                case ShipType.YT_1300:
                    name = "YT-1300";
                    break;
                case ShipType.YT_2400:
                    name = "YT-2400";
                    break;
                case ShipType.YV_666:
                    name = "YV-666";
                    break;
                case ShipType.Z_95_Headhunter:
                    name = "Z-95 Headhunter";
                    break;
                case ShipType.Aggressor:
                    name = "Aggressor";
                    break;
                case ShipType.C_ROC_Cruiser:
                    name = "C-ROC Cruiser";
                    break;
                case ShipType.Lancer_Class_Pursuit_Craft:
                    name = "Lancer-Class Pursuit Craft";
                    break;
                case ShipType.Raider_Class_Corvette:
                    name = "Raider-Class Corvette";
                    break;
                case ShipType.TIE_Advanced_Prototype:
                    name = "TIE Advanced Prototype";
                    break;
                case ShipType.TIE_Striker:
                    name = "TIE Striker";
                    break;
                case ShipType.U_Wing:
                    name = "U-Wing";
                    break;
                case ShipType.Quadjumper:
                    name = "Quadjumper";
                    break;
                case ShipType.Rebel_TIE_Fighter:
                    name = "TIE Fighter";
                    break;
                case ShipType.Upsilon_Class_Shuttle:
                    name = "Upsilon-Class Shuttle";
                    break;
                default:
                    name = "";
                    break;
            }

            switch (shipType)
            {
                case ShipType.A_Wing:
                case ShipType.ARC_170:
                case ShipType.Attack_Shuttle:
                case ShipType.B_Wing:
                case ShipType.E_Wing:
                case ShipType.G_1A_Starfighter:
                case ShipType.HWK_290:
                case ShipType.K_Wing:
                case ShipType.Kihraxz_Fighter:
                case ShipType.M3_A_Interceptor:
                case ShipType.Protectorate_Starfighter:
                case ShipType.StarViper:
                case ShipType.T_70_X_Wing:
                case ShipType.TIE_Advanced:
                case ShipType.TIE_Bomber:
                case ShipType.TIE_Defender:
                case ShipType.TIE_Fighter:
                case ShipType.TIE_Interceptor:
                case ShipType.TIE_Phantom:
                case ShipType.TIE_Punisher:
                case ShipType.TIE_FO_Fighter:
                case ShipType.TIE_SF_Fighter:
                case ShipType.X_Wing:
                case ShipType.Y_Wing:
                case ShipType.Z_95_Headhunter:
                case ShipType.TIE_Advanced_Prototype:
                case ShipType.TIE_Striker:
                case ShipType.Quadjumper:
                case ShipType.Rebel_TIE_Fighter:
                    shipSize = ShipSize.Small;
                    break;
                case ShipType.Upsilon_Class_Shuttle:
                case ShipType.U_Wing:
                case ShipType.VT_49_Decimator:
                case ShipType.Firespray_31:
                case ShipType.Lancer_Class_Pursuit_Craft:
                case ShipType.JumpMaster_5000:
                case ShipType.YT_1300:
                case ShipType.YT_2400:
                case ShipType.Lambda_Class_Shuttle:
                case ShipType.Aggressor:
                case ShipType.YV_666:
                case ShipType.VCX_100:
                    shipSize = ShipSize.Large;
                    break;
                case ShipType.C_ROC_Cruiser:
                case ShipType.GR_75_Medium_Transport:
                case ShipType.Gozanti_Class_Cruiser:
                case ShipType.Raider_Class_Corvette:
                case ShipType.CR90_Corvette:
                    shipSize = ShipSize.Huge;
                    break;
                default:
                    shipSize = ShipSize.Small;
                    break;
            }
        }
    }
}
