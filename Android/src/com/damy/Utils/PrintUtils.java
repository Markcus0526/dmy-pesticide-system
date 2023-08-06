package com.damy.Utils;

import android.content.res.Resources;
import com.damy.datatypes.STSaleCatalogInfo;
import com.damy.datatypes.STSaleCatalogPrintInfo;
import com.damy.datatypes.STSaleRejectInfo;
import com.damy.datatypes.STSaleRejectPrintInfo;
import com.printer.bluetooth.android.*;
import java.util.Calendar;

/**
 * Created with IntelliJ IDEA.
 * User: KHM
 * Date: 14-3-24
 * Time: 上午11:34
 * To change this template use File | Settings | File Templates.
 */
public class PrintUtils {
	public static int printSaleCatalogInfo(Resources resources, STSaleCatalogPrintInfo data, BluetoothPrinter printer)
	{
		int nResult = PrinterErrMgr.ERR_NONE;
		printer.init();

		if (printer.isPrinterNull())
			return PrinterErrMgr.ERR_PRINTER_NULL;

		if (!printer.isConnected())
			return PrinterErrMgr.ERR_PRINTER_NOT_CONNECTED;

		// Print empty lines for margin
		{
			printer.setCharacterMultiple(0, 0);
			printer.printText("\n\n\n");
		}

		// Print title
		{
			printer.setPrinter(BluetoothPrinter.COMM_ALIGN, BluetoothPrinter.COMM_ALIGN_CENTER);
			printer.setCharacterMultiple(0, 1);
			printer.printText("销售单" + "\n\n");
		}

		// Print content
		{
			printer.setPrinter(BluetoothPrinter.COMM_ALIGN, BluetoothPrinter.COMM_ALIGN_LEFT);
			printer.setCharacterMultiple(0, 0);
			String szText = "";

			szText += "选择仓库：" + data.store_name + "\n";
			szText += "经销商  ：" + data.shop_name + "\n";
			szText += "录入单号：" + data.ticket_name + "\n";
			szText += "客户名称：" + data.customer_name + "\n";
			szText += "手机号  ：" + data.phone_number + "\n\n";

			int i, cnt = data.catalog_info.size();
			for ( i = 0; i < cnt; i++ )
			{
				STSaleCatalogInfo item = data.catalog_info.get(i);
				szText += "产品名称：" + item.catalog_name + "\n";
				szText += "规格  ：" + item.standard_string + "\n";
				szText += "批号  ：" + item.largenumber + "\n";
				szText += "单价  ：" + item.oneprice + "\n";
				szText += "数量  ：" + item.quantity + "\n";
				szText += "金额  ：" + item.totalprice + "\n\n";
			}
			printer.printText(szText);

		}

		// Print empty lines for margin
		{
			printer.setCharacterMultiple(0, 0);
			printer.printText("\n\n\n");
		}

		return nResult;
	}


	public static int printSaleRejectInfo(Resources resources, STSaleRejectPrintInfo data, BluetoothPrinter printer)
	{
		int nResult = PrinterErrMgr.ERR_NONE;
		printer.init();

		if (printer.isPrinterNull())
			return PrinterErrMgr.ERR_PRINTER_NULL;

		if (!printer.isConnected())
			return PrinterErrMgr.ERR_PRINTER_NOT_CONNECTED;

		// Print empty lines for margin
		{
			printer.setCharacterMultiple(0, 0);
			printer.printText("\n\n\n");
		}

		// Print title
		{
			printer.setPrinter(BluetoothPrinter.COMM_ALIGN, BluetoothPrinter.COMM_ALIGN_CENTER);
			printer.setCharacterMultiple(0, 1);
			printer.printText("退货单" + "\n\n");
		}

		// Print content
		{
			printer.setPrinter(BluetoothPrinter.COMM_ALIGN, BluetoothPrinter.COMM_ALIGN_LEFT);
			printer.setCharacterMultiple(0, 0);
			String szText = "";

			szText += "选择仓库：" + data.store_name + "\n";
			szText += "经销商  ：" + data.shop_name + "\n";
			szText += "录入单号：" + data.ticket_name + "\n";
			szText += "客户名称：" + data.customer_name + "\n";
			szText += "手机号  ：" + data.phone_number + "\n\n";

			int i, cnt = data.reject_info.size();
			for ( i = 0; i < cnt; i++ )
			{
				STSaleRejectInfo item = data.reject_info.get(i);
				szText += "产品名称：" + item.catalog_name + "\n";
				szText += "规格  ：" + item.standard_name + "\n";
				szText += "批号  ：" + item.largenumber + "\n";
				szText += "单价  ：" + item.oneprice + "\n";
				szText += "数量  ：" + item.quantity + "\n";
				szText += "金额  ：" + item.totalprice + "\n\n";
			}
			printer.printText(szText);

		}

		// Print empty lines for margin
		{
			printer.setCharacterMultiple(0, 0);
			printer.printText("\n\n\n");
		}

		return nResult;
	}
}
