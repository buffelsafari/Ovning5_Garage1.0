using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Garage10.Garage
{
    enum LogMessage
    {
        VEHICLE_ALREADY_PARKED,
        PARKING_OCCUPIED,
        VEHICLE_PARKED,
        VEHICLE_REMOVED,
        PARKING_ALREADY_EMPTY,
        NO_PARKING_AVAILABLE,
        SYNTAX_ERROR,
        INVALID_VEHICLE_PARAMETERS,
        GARAGE_CREATED,
        PARKING_FEATURE_ADDED,
        COULD_NOT_ADD_FEATURE,
        OUT_OF_RANGE,
        NOT_A_VALID_FEATURE,
        PARKING_FEATURE_REMOVED,
        COULD_NOT_REMOVE_FEATURE,
        FILE_LOADED,
        FILE_SAVED,
        IO_ERROR

    }

    enum MessageType
    { 
        FAILURE,
        SUCCESS,
        INFORMATION,

    }
}
