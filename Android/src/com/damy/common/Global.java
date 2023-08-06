package com.damy.common;


import android.content.Context;
import android.content.SharedPreferences;
import android.graphics.Point;
import android.graphics.Rect;
import android.os.Build;
import android.view.Display;
import android.view.Window;
import android.view.WindowManager;
import com.damy.datatypes.*;
import android.app.Activity;
import android.app.AlertDialog;
import org.json.JSONObject;

public class Global {
	
	public static long 					Cur_UserId = 0;
	public static String 				Cur_UserName = "";
	public static String 				Cur_UserLoginId = "";
	public static long 					Cur_ShopId = 0;
	public static String 				Cur_ShopName = "";
	public static String 				Cur_UserRole = "";
	public static int                   Cur_Type = 0;
	
	public static int					Cur_AdminRole = 0;
	public static long					Cur_AdminRegionId = 0;
	
	public static int 					PAGE_SIZE = 10;
	
	public static STBuyCatalogInfo		BuyCatalog_SelectedItem = new STBuyCatalogInfo();
	public static boolean				BuyCatalog_isSelected = false;
	
	public static STStoreUsingInfo		StoreUsing_SelectItem = new STStoreUsingInfo();
	public static boolean				SotreUsing_isSelected = false;
	
	public static STSaleCatalogInfo		SaleCatalog_SelectItem = new STSaleCatalogInfo();
	public static boolean				SaleCatalog_isSelected = false;
	
	public static STSaleRejectInfo		SaleReject_SelectItem = new STSaleRejectInfo();
	public static boolean				SaleReject_isSelected = false;

	public static boolean savePrinter(Context appContext, STPrinter printer)
	{
		boolean bSuccess = true;
		SharedPreferences prefs = null;

		try
		{
			prefs = appContext.getSharedPreferences("jinwutong_info", Context.MODE_PRIVATE);
			SharedPreferences.Editor edit = prefs.edit();
			edit.putString("printer", STPrinter.EncodeToJSON(printer).toString());
			edit.commit();
		}
		catch (Exception ex)
		{
			ex.printStackTrace();
			bSuccess = false;
		}

		return bSuccess;
	}

	public static STPrinter loadPrinter(Context appContext)
	{
		String szPrinter = "";

		SharedPreferences prefs = appContext.getSharedPreferences("nongyao_info", Context.MODE_PRIVATE);
		szPrinter = prefs.getString("printer", "");

		if (szPrinter.equals(""))
			return null;

		try {
			return STPrinter.DecodeFromJSON(new JSONObject(szPrinter));
		} catch (Exception ex) {
			ex.printStackTrace();
			return null;
		}
	}

	public static int statusBarHeight(Activity activity) {
		Rect rectgle= new Rect();
		Window window= activity.getWindow();
		window.getDecorView().getWindowVisibleDisplayFrame(rectgle);
		return rectgle.top;
	}

	public static int getSystemVersion()
	{
		int nVersion = 0;

		try
		{
			nVersion = Build.VERSION.SDK_INT;
		}
		catch (Exception ex)
		{
			ex.printStackTrace();
			nVersion = -1;
		}

		return nVersion;
	}

	public static Point getScreenSize(Context appContext)
	{
		Point ptSize = new Point(0, 0);

		WindowManager wm = (WindowManager)appContext.getSystemService(Context.WINDOW_SERVICE);
		Display display = wm.getDefaultDisplay();

		if (getSystemVersion() >= Build.VERSION_CODES.HONEYCOMB_MR2)
			display.getSize(ptSize);
		else
			ptSize = new Point(display.getWidth(), display.getHeight());

		return ptSize;
	}

}

