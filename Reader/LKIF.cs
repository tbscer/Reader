using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;


namespace Reader
{
    public class LKIF
    {
        // Return Code List
        [Flags]
        public enum RC {	
	        RC_OK = 0x0000,				// The operation is completed successfully.
	        ///////////////////////////////////////////////
	        // Communication error from controller notification
	        //
	        RC_NAK_COMMAND = 0x1001,	// Command error
	        RC_NAK_COMMAND_LENGTH,		// Command length error
	        RC_NAK_TIMEOUT,				// Timeout
	        RC_NAK_CHECKSUM,			// Check sum error
	        RC_NAK_INVALID_STATE,		// Status error
	        RC_NAK_OTHER,				// Other error
	        RC_NAK_PARAMETER,			// Parameter error
	        RC_NAK_OUT_STAGE,			// OUT calculation count limitation error
	        RC_NAK_OUT_HEAD_NUM,		// No. of used head/OUT over error
	        RC_NAK_OUT_INVALID_CALC,	// OUT which cannot be used for calculation was specified for calculation.
	        RC_NAK_OUT_VOID,			// OUT which specified for calculation is not found.
	        RC_NAK_INVALID_CYCLE,		// Unavailable sampling cycle
	        RC_NAK_CTRL_ERROR,			// Main unit error
	        RC_NAK_SRAM_ERROR,			// Setting value error
	        ///////////////////////////////////////////////
	        // Communication DLL error notification
	        //
	        RC_ERR_OPEN_DEVICE = 0x2000,// Opening the device failed.
	        RC_ERR_NO_DEVICE,			// The device is not open.
	        RC_ERR_SEND,				// Command sending error
	        RC_ERR_RECEIVE,				// Response receiving error
	        RC_ERR_TIMEOUT,				// Timeout
	        RC_ERR_NODATA,				// No data
	        RC_ERR_NOMEMORY,			// No free memory
	
	        RC_ERR_DISCONNECT,			// Cable disconnection suspected
	        RC_ERR_UNKNOWN,				// Undefined error
        } 

        [Flags]
        public enum LKIF_FLOATRESULT {	
	        LKIF_FLOATRESULT_VALID,			// valid data
	        LKIF_FLOATRESULT_RANGEOVER_P,	// over range at positive (+) side
	        LKIF_FLOATRESULT_RANGEOVER_N,	// over range at negative (-) side
	        LKIF_FLOATRESULT_WAITING,		// Wait for comparator result
	        LKIF_FLOATRESULT_ALARM,			// alarm
	        LKIF_FLOATRESULT_INVALID,		// Invalid (error, etc.)
        }

        public struct LKIF_FLOATVALUE_OUT {
	        int					OutNo;			// OUT No(0-11)
	        LKIF_FLOATRESULT	FloatResult;	// valid or invalid data
	        float				Value;			// Measurement value
        } 

        [DllImport("LKIF2.dll", CallingConvention=CallingConvention.Winapi)]
        public static extern RC LKIF2_GetNumOfUsedAnalogCh(ref int NumOfUsedAnalogCh);

        [DllImport("LKIF2.dll", CallingConvention=CallingConvention.Winapi)]
        public static extern RC  LKIF2_OpenDeviceUsb();

        [DllImport("LKIF2.dll", CallingConvention=CallingConvention.Winapi, BestFitMapping=true)]
        public static extern RC LKIF2_GetCalcDataSingle(int OutNo,ref LKIF_FLOATVALUE_OUT CalcData);
    }
}
