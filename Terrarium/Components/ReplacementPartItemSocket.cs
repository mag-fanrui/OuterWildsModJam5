using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terrarium.Enums;

namespace Terrarium.Components
{
    public class ReplacementPartItemSocket : OWItemSocket
    {
        public TerrariumParamType Parameter;

        public override void Awake()
        {
            base.Awake();
            _acceptableType = ReplacementPartItem.PARAM_ITEM_TYPES[Parameter];
        }

        public override bool PlaceIntoSocket(OWItem item)
        {
            if (base.PlaceIntoSocket(item))
            {
                SetParameterEnable(true);
                return true;
            }
            return false;
        }

        public override OWItem RemoveFromSocket()
        {
            var item = base.RemoveFromSocket();
            SetParameterEnable(false);
            return item;
        }

        void SetParameterEnable(bool enabled)
        {
            var tc = TerrariumController.Instance;
            switch (Parameter)
            {
                case TerrariumParamType.SunDistance:
                    tc.SunDistanceEnabled.Value = enabled;
                    break;
                case TerrariumParamType.SunAngle:
                    tc.SunAngleEnabled.Value = enabled;
                    break;
                case TerrariumParamType.Humidity:
                    tc.HumidityEnabled.Value = enabled;
                    break;
                case TerrariumParamType.Atmosphere:
                    tc.AtmosphereEnabled.Value = enabled;
                    break;
            }
        }
    }
}
