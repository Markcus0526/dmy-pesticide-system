package com.damy.Utils;

/**
 * Created with IntelliJ IDEA.
 * User: KHM
 * Date: 14-3-25
 * Time: 下午5:47
 * To change this template use File | Settings | File Templates.
 */
public class PrinterErrMgr {
	public static final int ERR_NONE = 0;
	public static final int ERR_PRINTER_NOT_CONNECTED = -1;
	public static final int ERR_PRINTER_NULL = -2;

	public static String ErrCode2Msg(int nCode)
	{
		String szMsg = "";

		switch (nCode)
		{
			case ERR_NONE:                                          szMsg = "操作成功！";
			case ERR_PRINTER_NOT_CONNECTED:                       szMsg = "打印机未连接！";
			case ERR_PRINTER_NULL:                                 szMsg = "打印机未连接！";
			default:                                                 szMsg = "无法知道原因的错误！";
		}

		return szMsg;
	}
}
