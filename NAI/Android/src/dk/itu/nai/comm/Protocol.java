package dk.itu.nai.comm;

public enum Protocol  {

	/**
	 * PIN_CODE is followed by a byte defining the length of the pincode.
	 */
	PIN_CODE(1),
	
	/** 
	 * COLOR_CODE is followed by a byte defining the length of the pincode. *  
	 */         
	COLOR_CODE(2), 

	/**
	 *   USER_ID is followed by a int defining the length of the byte array to read. 
	 */
	USER_ID(3),  
	/** 
	 * To notify the server that Calibration should be persisted
	 */
	CALIBRATION_ACCEPTED(4), 

	/**
	 * To notify the phone client that the code was accepted
	 */
	PAIRING_CODE_ACCEPTED(5),
	/** 
	 * Server sends this to notify that the pincode  recieved was not accepted.
	 * This allows the client to take proper action (show the pincode again!). 
	 */
	PINCODE_REJECTED(6),

	/** 
	 *  Prefixes every picture send after PICTURE_STREAM_START.
	 *   Followed by an int designating the length of the picture byte array. 
	 */
	FRAME(10),
	
	
	/**
	 * Authentication
	 */
	
	AUTHENTICATION(20),
	AUTHENTICATION_ACCEPTED(21),
	AUTHENTICATION_REJECTED(22),

	/* - - - - - Event prefixes - - - - */

	/**
	 *  Client sends command to server when user touches screen.
	 *  Command followed by two doubles x, y (each a 4 byte representation) between 0.0 and 1.0,
	 *  designating the relative position on the screen. 
	 */
	TOUCH_DOWN(50),
	
	/** 
	 * Client sends command to server when user moves the finger on the screen.
	 *  Command followed by two doubles x, y (each a 4 byte representation) between 0.0 and 1.0,
	 *  designating the relative position on the screen. 
	 */
	TOUCH_MOVE(51),

	/**
	 * Client sends command to server when user lifts the finger of screen.
	 *  Command followed by two doubles x, y (each a 4 byte representation) between 0.0 and 1.0,
	 *  designating the last relative position on the screen.
	 */
	TOUCH_UP(52),

	/** 
	 * Server notifies the client that a picture stream starts.
	 */
	PICTURE_STREAM_START(60),

	/**
	 *	Server notifies the client that a picture stream stops. 
	 */
	PICTURE_STREAM_STOP(61),

	/** 
	 * Client notifies the server that the reading on the connection is paused. 
	 */
	CLIENT_PAUSED(70),
	
	/**
	 *  Client notifies the server that the reading on the connection is resumed.
	 */
	CLIENT_RESUMED(71),

	/* - - - - - request prefixes - - - -*/

	/** 
	 *  Server sends command to client, so that it knows what is expected.
	 *   Followed by 2 bytes defining length for PIN_CODE, and COLOR_CODE. 
	 */
	REQUEST_PAIRING_CODE(100),

	/** 
	 * Client sends command to server, to initiate the calibration mode.       
	 */
	REQUEST_CALIBRATION(101);


	private int _code;

	private Protocol(int b)
	{
		_code = b;
	}

	public byte getCommandByte()
	{
		return (byte) (_code & 255);
	}
	
	public static Protocol createFromByte(byte b)
	{
		Protocol[] all = Protocol.values();
		for (Protocol p : all)
			if (p.getCommandByte() == b)
				return p;	
		return null;
	}
}
