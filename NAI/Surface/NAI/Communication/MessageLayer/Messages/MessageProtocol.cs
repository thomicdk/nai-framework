
namespace NAI.Communication.MessageLayer.Messages
{
    internal enum MessageProtocol : byte
    {

        // Response prefixes
        
        ///<summary>
        /// PIN_CODE is followed by a byte defining the length of the pincode.     
        ///</summary>
        PIN_CODE = 1,
        /// <summary>
        /// COLOR_CODE is followed by a byte defining the length of the pincode. 
        /// </summary>         
        COLOR_CODE = 2, 
        /// <summary>
        /// 
        /// To notify the server that Calibration should be persisted
        /// </summary>
        CALIBRATION_ACCEPTED=  4, 
        /// <summary>
        /// To notify the phone client that the code was accepted
        /// </summary>
        PAIRING_CODE_ACCEPTED = 5,

        /// <summary>
        /// Server sends this to notify that the pincode  recieved was not accepted. 
        /// This allows the client to take proper action (show the pincode again!).
        /// </summary>
        PINCODE_REJECTED = 6,


        /// <summary>
        /// Prefixes every picture send after PICTURE_STREAM_START. 
        /// Followed by an int designating the length of the picture byte array.
        /// </summary>
        FRAME = 10,

        /// <summary>
        /// When client sends credentials
        /// </summary>
        CREDENTIALS = 20,

        /// <summary>
        /// Response sent to client when credentials are successfully authenticated
        /// </summary>
        AUTHENTICATION_ACCEPTED = 21,

        /// <summary>
        /// Response sent to client when credentials are not successfully authenticated
        /// </summary>
        AUTHENTICATION_REJECTED = 22,


        // Event prefixes
        
        /// <summary>
        /// Client sends command to server when user touches screen. 
        /// Command followed by two floats x, y (each a 4 byte representation) between 0.0 and 1.0,
        /// designating the relative position on the screen.
        /// </summary>
        TOUCH_DOWN = 50,
        /// <summary>
        /// Client sends command to server when user moves the finger on the screen. 
        /// Command followed by two floats x, y (each a 4 byte representation) between 0.0 and 1.0,
        /// designating the relative position on the screen.
        /// </summary>
        TOUCH_MOVE = 51,
        /// <summary>
        /// Client sends command to server when user lifts the finger of screen. 
        /// Command followed by two floats x, y (each a 4 byte representation) between 0.0 and 1.0,
        /// designating the last relative position on the screen.
        /// </summary>
        TOUCH_UP = 52,

        /// <summary>
        /// Server notifies the client that a picture stream starts.
        /// </summary>
        PICTURE_STREAM_START = 60,

        /// <summary>
        /// Server notifies the client that a picture stream stops.
        /// </summary>
        PICTURE_STREAM_STOP = 61,

        /// <summary>
        /// Client notifies the server that the reading on the connection is paused. 
        /// </summary>
        CLIENT_PAUSED = 70,
        /// <summary>
        /// Client notifies the server that the reading on the connection is resumed.
        /// </summary>
        CLIENT_RESUMED = 71,

        // request prefixes

        /// <summary>
        /// Server sends command to client, so that it knows what is expected. 
        /// Followed by 2 bytes defining length for PIN_CODE, and COLOR_CODE.
        /// </summary>        
        REQUEST_PAIRING_CODE = 100,
        /// <summary>
        /// Client sends command to server, to initiate the calibration mode.       
        /// </summary>        
        REQUEST_CALIBRATION = 101        

    }

}
