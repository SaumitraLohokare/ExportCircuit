using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileConverter.Utility
{
    public class Maps
    {
        public static Dictionary<string, string> PropertyNewName = new Dictionary<string, string>()
        {
            {"groupid", "GroupId" },
            {"bustype", "BusType" },
            {"parent", "ParentNode" },
            {"pt_phase", "ParticipatingPhases" },
            {"phases_connected", "ConnectedPhases" },
            {"dcircuit", "Feeder"},
            {"x", "Longitude"},
            {"y", "Latitude"},
            {"VAr_set_high", "ReactivePowerSetHigh"},
            {"VAr_set_low", "ReactivePowerSerLow"},
            {"dem_resp_lb", "DemandResponseLowerBound"},
            {"dem_resp_ub", "DemandResponseUpperBound"},
            {"dem_resp_shortage_cost", "DemandResponseShortageCost"},
            {"dem_resp_surplus_cost", "DemandResponseSurplusCost"},
            {"fr", "From"},
            {"CT_phase", "CurrentTransducerPhase"},
            {"PT_phase", "PowerTransducerPhase"},
            {"tap_pos_A", "TapPositionA"},
            {"tap_pos_B", "TapPositionB"},
            {"tap_pos_C", "TapPositionC"},
            {"max_kw", "MaxActivePower"},
            {"max_kvar", "MaxReactivePower"},
            {"max_kwh", "MaxEnergy"},
            {"base_kw_A", "BaseActivePowerA"},
            {"base_kw_B", "BaseActivePowerB"},
            {"base_kw_C", "BaseActivePowerC"},
            {"kw_A", "ActivePowerA"},
            {"kw_B", "ActivePowerB"},
            {"kw_C", "ActivePowerC"}
        };

        public static Dictionary<string, string> SheetMap = new Dictionary<string, string>()
        {
            {"OverheadLineConfiguration", "line_configuration"},
            {"Solar", "pv"}
        };
    }
}
