using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Celestos.Web.Static
{
    public static class Enums
    {
        /***********************************************************************************************************/


        #region ----- Public ENUM ------


        public enum BACKGROUNDWORKER_STATE
        {
            START_WORK = 1,
            COMPLETE_WORK = 2
        };


        public enum SESSION_STATUS
        {
            UNKNOWN = -1,
            INVALID = 0,
            VALID = 1
        };


        public enum NOTE_TYPE
        {
            PERSONAL = 1,
            GENERAL = 2
        };


        public enum FLEETLOG_MISSION
        {
            ALL = 0,
            ATTACK = 1,
            ACS_ATTACK = 2,
            TRANSPORT = 3,
            DEPLOYMENT = 4,
            ACS_DEFEND = 5,
            ESPIONAGE = 6,
            COLONIZATION = 7,
            HARVEST = 8,
            MOON_DESTRUCTION = 9,
            EXPEDITION = 15
        };


        #endregion ----- Public ENUM ------


        /***********************************************************************************************************/

    }
}
