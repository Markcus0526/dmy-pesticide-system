package com.damy.nongyao;

import java.lang.reflect.Method;
import java.util.ArrayList;
import java.util.Timer;
import java.util.TimerTask;

import android.bluetooth.BluetoothAdapter;
import android.bluetooth.BluetoothDevice;
import android.content.BroadcastReceiver;
import android.content.Context;
import android.content.IntentFilter;
import android.os.Handler;
import android.os.Message;
import android.widget.*;
import com.damy.Utils.*;
import com.damy.datatypes.*;
import com.printer.bluetooth.android.BluetoothPrinter;
import com.printer.bluetooth.android.PrinterType;
import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;

import com.damy.adapters.DialogSelectAdapter;
import com.damy.adapters.SaleCatalogAdapter;
import com.damy.backend.HttpConnUsingJSON;
import com.damy.backend.LoadResponseThread;
import com.damy.backend.ResponseData;
import com.damy.backend.ResponseRet;
import com.damy.common.Global;

import com.google.zxing.client.android.CaptureActivity;

import android.os.Bundle;
import android.content.Intent;
import android.graphics.Color;
import android.graphics.drawable.ColorDrawable;
import android.text.Editable;
import android.text.TextWatcher;
import android.view.Gravity;
import android.view.View;
import android.view.View.OnClickListener;
import android.view.View.OnFocusChangeListener;

public class SaleCatalogActivity extends BaseActivity {
	
	private enum REQ_TYPE{REQ_GETTICKETNUMBER, REQ_GETSTORELIST, REQ_SALECATALOG, REQ_GETCUSTOMERINFO};
	
	private PopupWindow 					dialog_store;
	private PopupWindow 					dialog_type;
	private PopupWindow 					popup_delconfirm;
    private LinearLayout					m_MaskLayer;
    private LinearLayout					m_PayLayer;
    
    private ArrayList<String>				m_typeList = new ArrayList<String>();
    private ArrayList<STStoreInfo> 		    m_StoreList = new ArrayList<STStoreInfo>();
    private ArrayList<STSaleCatalogInfo> 	m_SaleCatalogList = new ArrayList<STSaleCatalogInfo>();
  
    private ListView						m_lvSaleCatalogListView;
    private SaleCatalogAdapter				m_SaleCatalogAdapter;
    
    private AutoSizeTextView				txt_shopname;
    private AutoSizeTextView				txt_ticketnum;
    private AutoSizeTextView				txt_store_name;
    private AutoSizeEditText				edit_customername;
    private AutoSizeEditText				edit_customerphone;
    
    private AutoSizeTextView				txt_pay_paytype;
    private AutoSizeTextView				txt_pay_musttake;
    private AutoSizeEditText				txt_pay_remain;
    private AutoSizeEditText				edit_pay_realtake;
    
    private long 							m_CurStoreId = -1;
            
    private int								m_CurLongClickedItem = -1;
    
    private String							SaleCatalogListString = "";
    private String							m_CurSaveMoney = "";
    private String							m_CurSmallchangeMoney = "";
    private int								m_CurPayType = 0;
    private String							m_CurTicketNumber = "";
    private String							m_CurDate = "";
    
    private int								m_CurCustomerDataType = 0;
    private String							m_CurGetCustomerName = "";
    private String							m_CurGetCustomerPhone = "";
    
    private REQ_TYPE						m_reqType;

	private final int ACTIVITY_CODE_SELDEVICE = 0;
	private BluetoothAdapter mBtAdapter = null;
	public static BluetoothPrinter mPrinter = null;
	private BluetoothDevice mCurDevice = null;
	private IntentFilter mBoundFilter = new IntentFilter(BluetoothDevice.ACTION_BOND_STATE_CHANGED);
	private boolean mRegisteredReceiver = false, mConnectDevice = false, mConnecting = false;

	private String mMacAddr = "", mConnectedDeviceName = "";
	private boolean mIsThermal = true, mIs58mm = true, mIsRepair = false;
    
	@Override
	protected void onCreate(Bundle savedInstanceState) {
		super.onCreate(savedInstanceState);
		setContentView(R.layout.activity_sale_catalog);
		
		ResolutionSet._instance.iterateChild(findViewById(R.id.fl_salecatalog));
		
		Global.SaleCatalog_isSelected = false;
		
		initControls();
		setDialogTypeAdapter();
		readContents();
	}
	
	@Override
	protected void onResume()
	{
		super.onResume();
		setSaleCatalogAdapter();
	}
	
	private void initControls()
	{
		FrameLayout fl_backbtn = (FrameLayout)findViewById(R.id.fl_salecatalog_backbtn1);
		FrameLayout fl_homebtn = (FrameLayout)findViewById(R.id.fl_salecatalog_homebtn);
		FrameLayout fl_addbtn = (FrameLayout)findViewById(R.id.fl_salecatalog_addbtn);
		FrameLayout fl_okbtn = (FrameLayout)findViewById(R.id.fl_salecatalog_savebtn);
		FrameLayout fl_closebtn = (FrameLayout)findViewById(R.id.fl_salecatalog_cancelbtn);
		FrameLayout fl_printbtn = (FrameLayout)findViewById(R.id.fl_salecatalog_printbtn);
		
		fl_backbtn.setOnClickListener(new OnClickListener() {
        	public void onClick(View v) {
        		onClickBack();
        	}
        });
		
		fl_homebtn.setOnClickListener(new OnClickListener() {
        	public void onClick(View v) {
        		onClickHome();
        	}
        });
		
		fl_addbtn.setOnClickListener(new OnClickListener() {
        	public void onClick(View v) {
        		onClickAdd();
        	}
        });
		
		fl_okbtn.setOnClickListener(new OnClickListener() {
        	public void onClick(View v) {
        		onClickOk();
        	}
        });
		
		fl_closebtn.setOnClickListener(new OnClickListener() {
        	public void onClick(View v) {
        		onClickClose();
        	}
        });

		fl_printbtn.setOnClickListener(new OnClickListener() {
			@Override
			public void onClick(View v) {
				onClickPrint();
			}
		});

		// Init bluetooth
		mBtAdapter = BluetoothAdapter.getDefaultAdapter();
		if (mBtAdapter == null) {
			showToastMessage(getResources().getString(R.string.STR_BLUETOOTH_NOTABLE));
			return;
		}

		if (!mBtAdapter.isEnabled())
		{
			showToastMessage(getResources().getString(R.string.STR_BLUETOOTH_AUTOENABLE));
			mBtAdapter.enable();
		}

		IntentFilter filter = new IntentFilter();
		//filter.addAction(BluetoothAdapter.ACTION_STATE_CHANGED);
		//filter.addAction(BluetoothDevice.ACTION_ACL_CONNECTED);
		filter.addAction(BluetoothDevice.ACTION_ACL_DISCONNECT_REQUESTED);
		filter.addAction(BluetoothDevice.ACTION_ACL_DISCONNECTED);
		//filter.addAction(BluetoothDevice.ACTION_BOND_STATE_CHANGED);
		//filter.addAction(BluetoothDevice.ACTION_FOUND);
		//filter.addAction(BluetoothDevice.ACTION_CLASS_CHANGED);
		//filter.addAction(BluetoothDevice.ACTION_NAME_CHANGED);
		registerReceiver(mStateReceiver, filter);

		if (!mMacAddr.equals(""))
		{
			BluetoothDevice device = mBtAdapter.getRemoteDevice(mMacAddr);
			if (device == null)
			{
				showToastMessage(getResources().getString(R.string.STR_CONNECT_PRINTER_FAILED));
				mConnectDevice = false;
				mConnecting = false;
			}

			mConnectedDeviceName = device.getName();

			startProgress(getResources().getString(R.string.STR_NOW_CONNECTING));
			if (device.getBondState() == BluetoothDevice.BOND_NONE) {
				PairDevice(false, device);
			} else if(device.getBondState() == BluetoothDevice.BOND_BONDED) {
				initPrinter(device);
			}
		}
		////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	    txt_shopname = (AutoSizeTextView)findViewById(R.id.txt_salecatalog_shopname);
	    txt_ticketnum = (AutoSizeTextView)findViewById(R.id.txt_salecatalog_ticketnum);
	    txt_store_name = (AutoSizeTextView)findViewById(R.id.txt_buy_store);
	    edit_customername = (AutoSizeEditText)findViewById(R.id.edit_salecatalog_customername);
	    edit_customerphone = (AutoSizeEditText)findViewById(R.id.edit_salecatalog_customerphone);
	    
	    edit_customername.setOnFocusChangeListener(new OnFocusChangeListener() {
			@Override
			public void onFocusChange(View v, boolean hasFocus) {
				// TODO Auto-generated method stub
				if ( !hasFocus )
					onGetCustomerPhone();
			}
		});
	    
	    edit_customerphone.setOnFocusChangeListener(new OnFocusChangeListener() {
			@Override
			public void onFocusChange(View v, boolean hasFocus) {
				// TODO Auto-generated method stub
				if ( !hasFocus )
					onGetCustomerName();
			}
		});
		
		m_MaskLayer = (LinearLayout)findViewById(R.id.ll_salecatalog_masklayer);
		m_MaskLayer.setVisibility(View.INVISIBLE);
				
		m_lvSaleCatalogListView = (ListView)findViewById(R.id.anSaleCatalogContentView);
		m_SaleCatalogList = new ArrayList<STSaleCatalogInfo>();

        m_lvSaleCatalogListView.setCacheColorHint(Color.TRANSPARENT);
        m_lvSaleCatalogListView.setDividerHeight(0);
        m_lvSaleCatalogListView.setDrawSelectorOnTop(true);

        m_SaleCatalogAdapter = new SaleCatalogAdapter(SaleCatalogActivity.this, m_SaleCatalogList);
        m_lvSaleCatalogListView.setAdapter(m_SaleCatalogAdapter);
        
        m_lvSaleCatalogListView.setOnItemLongClickListener(new AdapterView.OnItemLongClickListener() {
			public boolean onItemLongClick(AdapterView<?> parent, View view, int position, long id) {
				onLongClickItem(parent, position);
				return true;
        	}
		});
		
		m_PayLayer = (LinearLayout)findViewById(R.id.ll_salecatalog_pay);
		m_PayLayer.setVisibility(View.INVISIBLE);
		
		FrameLayout fl_pay_backbtn = (FrameLayout)findViewById(R.id.fl_salepay_backbtn);
		FrameLayout fl_pay_homebtn = (FrameLayout)findViewById(R.id.fl_salepay_homebtn);
		FrameLayout fl_pay_okbtn = (FrameLayout)findViewById(R.id.fl_salepay_savebtn);
		FrameLayout fl_pay_remainbtn = (FrameLayout)findViewById(R.id.fl_salepay_closebtn);
		
		fl_pay_backbtn.setOnClickListener(new OnClickListener() {
        	public void onClick(View v) {
        		onClickPayBack();
        	}
        });
		
		fl_pay_homebtn.setOnClickListener(new OnClickListener() {
        	public void onClick(View v) {
        		onClickPayHome();
        	}
        });
		
		fl_pay_okbtn.setOnClickListener(new OnClickListener() {
        	public void onClick(View v) {
        		onClickPayOk();
        	}
        });
		
		fl_pay_remainbtn.setOnClickListener(new OnClickListener() {
        	public void onClick(View v) {
        		onClickCancel();
        	}
        });
		
		LinearLayout ll_pay_typesel = (LinearLayout)findViewById(R.id.ll_salepay_typesel);
		LinearLayout ll_store_sel = (LinearLayout)findViewById(R.id.ll_buy_store);
		ll_pay_typesel.setOnClickListener(new OnClickListener() {
        	public void onClick(View v) {
        		onClickPayTypeSel();
        	}
        });
		ll_store_sel.setOnClickListener(new OnClickListener(){
			public void onClick(View v) {
				onClickStoreName();
			}
		});
		
		txt_pay_paytype = (AutoSizeTextView)findViewById(R.id.txt_salepay_paytype);
		txt_pay_musttake = (AutoSizeTextView)findViewById(R.id.txt_salepay_musttakemoney);
		txt_pay_remain = (AutoSizeEditText)findViewById(R.id.txt_salepay_remainmoney);
		edit_pay_realtake = (AutoSizeEditText)findViewById(R.id.txt_salepay_realtakemoney);
	}

	private void readContents()
	{
		txt_shopname.setText(Global.Cur_ShopName);
		
		m_reqType = REQ_TYPE.REQ_GETTICKETNUMBER;
		new LoadResponseThread(SaleCatalogActivity.this).start();
	}
	
	private void onClickBack()
	{
		finish();
	}
	
	private void onClickHome()
	{
		Intent main_activity = new Intent(this, MainActivity.class);
		startActivity(main_activity);	
		finish();
	}
	
	private void onClickAdd()
	{
		Global.SaleCatalog_isSelected = false;
		Intent salecatalog_add_activity = new Intent(this, SaleCatalogAddActivity.class);
		salecatalog_add_activity.putExtra(SaleCatalogAddActivity.SALECATALOG_ADD_STOREID, m_CurStoreId);
		salecatalog_add_activity.putExtra(SaleCatalogAddActivity.SALECATALOG_ADD_STORENAME, txt_store_name.getText().toString());
		startActivity(salecatalog_add_activity);
	}
	
	private void onClickOk()
	{
		if ( edit_customername.getText().toString().length() == 0 )
		{
			showToastMessage(getResources().getString(R.string.error_required_customername));
			return;
		}
		
		if ( edit_customerphone.getText().toString().length() == 0 )
		{
			showToastMessage(getResources().getString(R.string.error_required_customerphone));
			return;
		}
		
		if ( edit_customerphone.getText().toString().length() != 11 )
		{
			showToastMessage(getResources().getString(R.string.error_mobilephonenum_charactercount));
			return;
		}
		
		int cnt = m_SaleCatalogList.size();
		double musttake = 0.0;
		
		if ( cnt == 0 )
		{
			showToastMessage(getResources().getString(R.string.error_nosalecatalog));
			return;
		}
		
		for ( int i = 0; i < cnt; i++ )
		{
			musttake += m_SaleCatalogList.get(i).totalprice;
		}
		
		txt_pay_musttake.setText(String.valueOf(musttake));
		txt_pay_paytype.setText(getResources().getString(R.string.common_actual));
		txt_pay_remain.setText("");
		edit_pay_realtake.setText("");
		m_CurPayType = 0;
		
		m_PayLayer.setVisibility(View.VISIBLE);
	}
	
	private void onClickClose()
	{
		readContents();
		
		m_SaleCatalogList.clear();
		m_SaleCatalogAdapter.notifyDataSetChanged();
		
		edit_customername.setText("");
		edit_customerphone.setText("");
	}

	// Printer receivers
	private BroadcastReceiver mStateReceiver = new BroadcastReceiver() {
		public void onReceive(Context context, Intent intent) {
			String action = intent.getAction();
			BluetoothDevice device = intent.getParcelableExtra(BluetoothDevice.EXTRA_DEVICE);

			if (action.equals(BluetoothDevice.ACTION_ACL_DISCONNECTED)) {
				if(mPrinter != null && device != null && mConnectDevice){
					if(mPrinter.getMacAddress().equals(device.getAddress()))
						mPrinter.closeConnection();
				}
			}

			//setButtonState();
		}
	};

	private final Handler mHandler = new Handler() {
		@Override
		public void handleMessage(Message msg) {
			switch (msg.what) {
				case BluetoothPrinter.Handler_Connect_Connecting:
					mConnecting = true;
					mConnectDevice = false;
					break;
				case BluetoothPrinter.Handler_Connect_Success:
					stopProgress();
					showToastMessage(getResources().getString(R.string.STR_CONNECT_PRINTER_SUCCESS));
					mConnectDevice = true;
					mConnecting = false;
					Global.savePrinter(getApplicationContext(), new STPrinter(mPrinter.getPrinterName(), mPrinter.getMacAddress()));
					break;
				case BluetoothPrinter.Handler_Connect_Failed:
					stopProgress();
					showToastMessage(getResources().getString(R.string.STR_CONNECT_PRINTER_FAILED));
					mConnectDevice = false;
					mConnecting = false;
					break;
				case BluetoothPrinter.Handler_Connect_Closed:
				{
					TimerTask timertask = new TimerTask() {
						@Override
						public void run() {
							runOnUiThread(new Runnable() {
								@Override
								public void run() {
									stopProgress();
									mConnectDevice = false;
									mConnecting = false;
									showToastMessage(getResources().getString(R.string.STR_DISCONNECT_PRINTER));
									//setButtonState();
								}
							});
						}
					};

					Timer timer = new Timer();
					timer.schedule(timertask, 2000);

				}
				break;
			}

			//setButtonState();
		}
	};

	private void initPrinter(BluetoothDevice device){
		mPrinter = new BluetoothPrinter(device);

		if (mIsThermal) {
			mPrinter.setCurrentPrintType(mIs58mm ? PrinterType.TIII : PrinterType.T9);
		} else {
			mPrinter.setCurrentPrintType(PrinterType.T5);
		}

		// set handler for receive message of connect state from sdk.
		mPrinter.setHandler(mHandler);
		mPrinter.openConnection();
		mPrinter.setEncoding("GBK");
	}

	private boolean PairDevice(boolean repair, BluetoothDevice device)
	{
		boolean success = false;

		try {
			if (!mRegisteredReceiver) {
				mCurDevice = device;
				registerReceiver(mBoundDeviceReceiver, mBoundFilter);
				mRegisteredReceiver = true;
			}

			if (repair)
			{
				Method removeBondMethod = BluetoothDevice.class.getMethod("removeBond");
				success = (Boolean)removeBondMethod.invoke(device);
			}
			else
			{
				Method createBondMethod = BluetoothDevice.class.getMethod("createBond");
				success = (Boolean)createBondMethod.invoke(device);
			}
		} catch (Exception e) {
			e.printStackTrace();
			success = false;
		}

		return success;
	}

	private BroadcastReceiver mBoundDeviceReceiver = new BroadcastReceiver()
	{
		public void onReceive(Context context, Intent intent)
		{
			String action = intent.getAction();

			if(BluetoothDevice.ACTION_BOND_STATE_CHANGED.equals(action))
			{
				BluetoothDevice device = intent.getParcelableExtra(BluetoothDevice.EXTRA_DEVICE);
				if(!mCurDevice.equals(device))
					return;

				switch (device.getBondState())
				{
					case BluetoothDevice.BOND_BONDING:
						break;
					case BluetoothDevice.BOND_BONDED:
						stopProgress();
						if(mRegisteredReceiver) {
							unregisterReceiver(mBoundDeviceReceiver);
							mRegisteredReceiver = false;
						}
						initPrinter(device);
						break;
					case BluetoothDevice.BOND_NONE:
						if (mIsRepair)
						{
							TimerTask timertask = new TimerTask() {
								@Override
								public void run() {
									runOnUiThread(new Runnable() {
										@Override
										public void run() {
											mIsRepair = false;
											PairDevice(false, mCurDevice);
										}
									});
								}
							};

							Timer timer = new Timer();
							timer.schedule(timertask, 1000);
						}
						else if (mRegisteredReceiver)
						{
							stopProgress();
							unregisterReceiver(mBoundDeviceReceiver);
							mRegisteredReceiver = false;
						}
					default:
						break;
				}
			}
		}
	};
	public STSaleCatalogPrintInfo getSaleCatalogPrintInfo()
	{
		STSaleCatalogPrintInfo printInfo = new STSaleCatalogPrintInfo();

		printInfo.store_name = txt_store_name.getText().toString();      //
		printInfo.shop_name = txt_shopname.getText().toString();           //
		printInfo.ticket_name = txt_ticketnum.getText().toString();                                //
		printInfo.customer_name = edit_customername.getText().toString();                                    //
		printInfo.phone_number = edit_customerphone.getText().toString();

		ArrayList<STSaleCatalogInfo> _catalog_info = new ArrayList<STSaleCatalogInfo>();
		int i, cnt = m_SaleCatalogList.size();
		for ( i = 0; i < cnt; i++ )
		{
			STSaleCatalogInfo item = m_SaleCatalogList.get(i);
			_catalog_info.add(item);
		}
		printInfo.catalog_info = _catalog_info;

		return printInfo;
	}

	private void onClickPrint()
	{
		if (mConnectDevice)
		{
			int nResult = PrintUtils.printSaleCatalogInfo(getResources(), getSaleCatalogPrintInfo(), mPrinter);
			if (nResult < 0) {
				showToastMessage(PrinterErrMgr.ErrCode2Msg(nResult));

				if (nResult == PrinterErrMgr.ERR_PRINTER_NOT_CONNECTED || nResult == PrinterErrMgr.ERR_PRINTER_NULL)
				{
					mConnectDevice = false;
					mConnecting = false;
				}
			}
		}
		else
		{
			Intent intent = new Intent(SaleCatalogActivity.this, SelectDeviceActivity.class);
			intent.putExtra("mac_addr", mMacAddr);
			intent.putExtra("name", mConnectedDeviceName);
			startActivityForResult(intent, ACTIVITY_CODE_SELDEVICE);
		}
	}

	private void onClickPayBack()
	{
		m_PayLayer.setVisibility(View.INVISIBLE);
	}
	
	private void onClickPayHome()
	{
		Intent main_activity = new Intent(this, MainActivity.class);
		startActivity(main_activity);	
		finish();
	}
	
	private void onClickPayOk()
	{
		if (edit_pay_realtake.getText().toString().length() == 0)
		{
			showToastMessage(getResources().getString(R.string.error_required_realtakemoney));
			return;
		}
		if (txt_pay_remain.getText().toString().length() == 0)
		{
			showToastMessage(getResources().getString(R.string.error_required_restmoney1));
			return;
		}
		
		double must = Double.valueOf(txt_pay_musttake.getText().toString());
		double remainmoney = Double.valueOf(edit_pay_realtake.getText().toString());
		double smallchange = Double.valueOf(txt_pay_remain.getText().toString());
		
		if ( remainmoney < 0 )
		{
			showToastMessage(getResources().getString(R.string.error_lackrealtakemoney));
			return;
		}
		if ( smallchange < 0 )
		{
			showToastMessage(getResources().getString(R.string.error_lackrestmoney));
			return;
		}
		if ( remainmoney + smallchange > must )
		{
			showToastMessage(getResources().getString(R.string.error_sumvaluewrong));
			return;
		}
		
		m_CurSaveMoney = String.valueOf(remainmoney);
		m_CurSmallchangeMoney = String.valueOf(smallchange);
		
		MakeSaleCatalogListString();
		
		m_reqType = REQ_TYPE.REQ_SALECATALOG;
		new LoadResponseThread(SaleCatalogActivity.this).start();
	}
	
	private void onGetCustomerPhone()
	{
		if ( edit_customername.getText().toString().length() == 0 )
			return;
		
		m_CurCustomerDataType = 0;
		m_reqType = REQ_TYPE.REQ_GETCUSTOMERINFO;
		new LoadResponseThread(SaleCatalogActivity.this).start();
	}
	
	private void onGetCustomerName()
	{
		if ( edit_customerphone.getText().toString().length() == 0 )
			return;
		
		m_CurCustomerDataType = 1;
		m_reqType = REQ_TYPE.REQ_GETCUSTOMERINFO;
		new LoadResponseThread(SaleCatalogActivity.this).start();
	}
	
	private void MakeSaleCatalogListString()
	{
		SaleCatalogListString = "";
		
		int i, cnt = m_SaleCatalogList.size();
		
		for ( i = 0; i < cnt; i++ )
		{
			STSaleCatalogInfo item = m_SaleCatalogList.get(i);
			SaleCatalogListString += String.valueOf(item.catalog_id) + "," + String.valueOf(item.standard_id) + "," + String.valueOf(item.largenumber) + "," + String.valueOf(item.oneprice) + "," + String.valueOf(item.quantity) + "@";
		}
	}
	
	private void onClickCancel()
	{
		m_PayLayer.setVisibility(View.INVISIBLE);
	}
	
	private void onClickPayTypeSel()
	{
		m_MaskLayer.setVisibility(View.VISIBLE);
		dialog_type.showAtLocation(findViewById(R.id.ll_salecatalog_masklayer), Gravity.CENTER_HORIZONTAL | Gravity.CENTER_VERTICAL, 0, 0);
	}
	
	private void onClickStoreName()
	{
		m_MaskLayer.setVisibility(View.VISIBLE);
		dialog_store.showAtLocation(findViewById(R.id.ll_salecatalog_masklayer), Gravity.CENTER_HORIZONTAL | Gravity.CENTER_VERTICAL, 0, 0);
	}
	
	private void setSaleCatalogAdapter()
	{
        STSaleCatalogInfo newitem = new STSaleCatalogInfo();
		if(Global.SaleCatalog_isSelected == true)
		{
			newitem.catalog_id = Global.SaleCatalog_SelectItem.catalog_id;
			newitem.catalog_name = Global.SaleCatalog_SelectItem.catalog_name;
			newitem.standard_id = Global.SaleCatalog_SelectItem.standard_id;
			newitem.standard_string = Global.SaleCatalog_SelectItem.standard_string;
			newitem.largenumber = Global.SaleCatalog_SelectItem.largenumber;
			newitem.oneprice = Global.SaleCatalog_SelectItem.oneprice;
			newitem.quantity = Global.SaleCatalog_SelectItem.quantity;
			newitem.totalprice = Global.SaleCatalog_SelectItem.totalprice;
					
			m_SaleCatalogList.add(newitem);
			m_SaleCatalogAdapter.notifyDataSetChanged();

			Global.SaleCatalog_isSelected = false;
		}
	}
	
	private void setDialogStoreAdapter()
	{
		View popupview = View.inflate(this, R.layout.dialog_select, null);
		ResolutionSet._instance.iterateChild(popupview);
		
		ArrayList<String> arGeneral = new ArrayList<String>();
		
		int cnt = m_StoreList.size();
		for ( int i = 0; i < cnt; i++ )
			arGeneral.add(m_StoreList.get(i).name);
		
		DialogSelectAdapter Adpater = new DialogSelectAdapter(this, arGeneral);
		
		ListView list = (ListView)popupview.findViewById(R.id.lv_dialog_listview);
		list.setAdapter(Adpater);
		list.setDrawSelectorOnTop(true);
		list.setDivider(new ColorDrawable(getResources().getColor(R.color.dialog_line)));
		list.setCacheColorHint(Color.TRANSPARENT);
        list.setDividerHeight(1);
		
		list.setOnItemClickListener(new AdapterView.OnItemClickListener() {
			public void onItemClick(AdapterView<?> parent, View view, int position, long id) {
				onClickStoreItem(position);
        	}
		});
		
		dialog_store = new PopupWindow(popupview, R.dimen.common_popup_dialog_width, R.dimen.common_popup_dialog_height,true);
		dialog_store.setAnimationStyle(-1);
		
		if ( m_StoreList.size() > 0 )
		{
			m_CurStoreId = m_StoreList.get(0).id;
			txt_store_name.setText(m_StoreList.get(0).name);
		}
		else
			txt_store_name.setText("");
	}
	
	private void setDialogTypeAdapter()
	{
		View popupview = View.inflate(this, R.layout.dialog_select, null);
		ResolutionSet._instance.iterateChild(popupview);
		
		AutoSizeTextView txt_title = (AutoSizeTextView)popupview.findViewById(R.id.txt_dialog_title);
		txt_title.setText(getResources().getString(R.string.dialog_select_takemoneytype));

        ImageView img_cancel = (ImageView) popupview.findViewById(R.id.img_dialog_cancel);
        img_cancel.setOnClickListener(new OnClickListener() {
            @Override
            public void onClick(View v) {
                if (dialog_type != null && dialog_type.isShowing() )
                {
                    m_MaskLayer.setVisibility(View.INVISIBLE);
                    dialog_type.dismiss();
                }
            }
        });

		m_typeList.add(getResources().getString(R.string.common_actual));
		m_typeList.add(getResources().getString(R.string.common_bill));
		m_typeList.add(getResources().getString(R.string.common_bankpayment));
		
		DialogSelectAdapter Adapter = new DialogSelectAdapter(this, m_typeList);
		
		ListView list = (ListView)popupview.findViewById(R.id.lv_dialog_listview);
		list.setAdapter(Adapter);
		list.setDrawSelectorOnTop(true);
		list.setDivider(new ColorDrawable(getResources().getColor(R.color.dialog_line)));
		list.setCacheColorHint(Color.TRANSPARENT);
        list.setDividerHeight(1);
		
		list.setOnItemClickListener(new AdapterView.OnItemClickListener() {
			public void onItemClick(AdapterView<?> parent, View view, int position, long id) {
				onClickTypeItem(position);
        	}
		});
		
		dialog_type = new PopupWindow(popupview, R.dimen.common_popup_dialog_width, R.dimen.common_popup_dialog_height,true);
		dialog_type.setAnimationStyle(-1);
	}
	
	private void onClickStoreItem(int pos)
	{
		m_MaskLayer.setVisibility(View.INVISIBLE);
		dialog_store.dismiss();
		
		m_CurStoreId = m_StoreList.get(pos).id;
		txt_store_name.setText(m_StoreList.get(pos).name);
		
		m_SaleCatalogList.clear();
		m_SaleCatalogAdapter.notifyDataSetChanged();
	}
	
	private void onClickTypeItem(int pos)
	{
		m_MaskLayer.setVisibility(View.INVISIBLE);
		dialog_type.dismiss();
		
		txt_pay_paytype.setText(m_typeList.get(pos));
		m_CurPayType = pos;
	}
	
	private void onLongClickItem(View view, int position)
	{
		m_CurLongClickedItem = position;
		
		m_MaskLayer.setVisibility(View.VISIBLE);
		
		View popupview = View.inflate(this, R.layout.dialog_delconfirm, null);
		ResolutionSet._instance.iterateChild(popupview);
		popup_delconfirm = new PopupWindow(popupview, R.dimen.common_delconfirm_dialog_width, R.dimen.common_delconfirm_dialog_height,true);
		popup_delconfirm.setAnimationStyle(-1);
		
		popup_delconfirm.showAtLocation(findViewById(R.id.ll_salecatalog_masklayer), Gravity.CENTER_HORIZONTAL | Gravity.CENTER_VERTICAL, 0, 0);
		
		AutoSizeTextView txt_delconfirm_msg = (AutoSizeTextView)popupview.findViewById(R.id.txt_dialog_delconfirm_msg);
		txt_delconfirm_msg.setText(getResources().getString(R.string.confirm_del_salecatalog));
		
		FrameLayout fl_delconfirm_ok = (FrameLayout)popupview.findViewById(R.id.fl_dialog_delconfirm_okbtn);
		FrameLayout fl_delconfirm_cancel = (FrameLayout)popupview.findViewById(R.id.fl_dialog_delconfirm_cancelbtn);
		
		fl_delconfirm_ok.setOnClickListener(new OnClickListener() {
        	public void onClick(View v) {
        		onClickDelConfirmOk();
        	}
        });
		fl_delconfirm_cancel.setOnClickListener(new OnClickListener() {
        	public void onClick(View v) {
        		onClickDelConfirmCancel();
        	}
        });
	}
	
	private void onClickDelConfirmOk()
	{
		m_MaskLayer.setVisibility(View.INVISIBLE);
		popup_delconfirm.dismiss();
		
		m_SaleCatalogList.remove(m_CurLongClickedItem);
		m_SaleCatalogAdapter.notifyDataSetChanged();
	}
	
	private void onClickDelConfirmCancel()
	{
		m_MaskLayer.setVisibility(View.INVISIBLE);
		popup_delconfirm.dismiss();
	}

	public void refreshUI() {
		super.refreshUI();
		
		if ( m_reqType == REQ_TYPE.REQ_GETTICKETNUMBER )
		{
			txt_ticketnum.setText(m_CurTicketNumber);
			
			m_reqType = REQ_TYPE.REQ_GETSTORELIST;
			new LoadResponseThread(SaleCatalogActivity.this).start();
		}
		else if ( m_reqType == REQ_TYPE.REQ_GETSTORELIST )
		{
			setDialogStoreAdapter();
		}
		else if ( m_reqType == REQ_TYPE.REQ_GETCUSTOMERINFO )
		{
			if ( m_CurCustomerDataType == 0 )
			{
				if ( m_CurGetCustomerPhone.length() > 0 )
					edit_customerphone.setText(m_CurGetCustomerPhone);
			}
			else
			{
				if ( m_CurGetCustomerName.length() > 0 )
					edit_customername.setText(m_CurGetCustomerName);
			}
		}
		else if ( m_reqType == REQ_TYPE.REQ_SALECATALOG )
		{
			if (m_nResponse == ResponseRet.RET_SUCCESS)
			{
				m_PayLayer.setVisibility(View.INVISIBLE);
				showToastMessage(getResources().getString(R.string.common_success));
				onClickClose();
			}
			else if ( m_nResponse == ResponseRet.RET_TICKETNUMUSED )
			{
				m_PayLayer.setVisibility(View.INVISIBLE);
				txt_ticketnum.setText(m_CurTicketNumber);
				txt_store_name.setText(m_CurDate);
			}
		}
	}
	
	public void getResponseJSON() {
		try {
			if ( m_reqType == REQ_TYPE.REQ_GETTICKETNUMBER )
			{
				m_nResponse = ResponseRet.RET_SUCCESS;
				
				String strRequest = HttpConnUsingJSON.REQ_GETTICKETNUMBER;
				strRequest += "?shop_id=" + String.valueOf(Global.Cur_ShopId);
				strRequest += "&type=2";
				
				JSONObject response = m_HttpConnUsingJSON.getGetJSONObject(strRequest);
				if (response == null) {
					m_nResponse = ResponseRet.RET_INTERNAL_EXCEPTION;
					return;
				}
				
				m_nResponse = response.getInt(ResponseData.RESPONSE_RET);
				if (m_nResponse == ResponseRet.RET_SUCCESS) {
					
					JSONObject dataObject = response.getJSONObject(ResponseData.RESPONSE_DATA);
					
					m_CurTicketNumber = dataObject.getString("data");
	            	m_CurDate = dataObject.getString("date");
				}
			}
			else if ( m_reqType == REQ_TYPE.REQ_GETSTORELIST )
			{
				m_nResponse = ResponseRet.RET_SUCCESS;
				
				String strRequest = HttpConnUsingJSON.REQ_GETSTORELIST;
				strRequest += "?shop_id=" + String.valueOf(Global.Cur_ShopId);
				
				JSONObject response = m_HttpConnUsingJSON.getGetJSONObject(strRequest);
				if (response == null) {
					m_nResponse = ResponseRet.RET_INTERNAL_EXCEPTION;
					return;
				}
				
				m_nResponse = response.getInt(ResponseData.RESPONSE_RET);
				if (m_nResponse == ResponseRet.RET_SUCCESS) {
	            	JSONObject dataObject = response.getJSONObject(ResponseData.RESPONSE_DATA);
					
					int count = dataObject.getInt("count");
		            JSONArray dataList = dataObject.getJSONArray("data");
		            
		            for (int i = 0; i < count; i++) {
		            	JSONObject tmpObj = (JSONObject) dataList.get(i);
			            STStoreInfo itemInfo = new STStoreInfo();

						itemInfo.id = tmpObj.getInt("store_id");								
						itemInfo.name = tmpObj.getString("name");							
						
						m_StoreList.add(itemInfo);
		            }
				}
			}
			else if ( m_reqType == REQ_TYPE.REQ_GETCUSTOMERINFO )
			{
				m_nResponse = ResponseRet.RET_SUCCESS;
				
				String strRequest = HttpConnUsingJSON.REQ_GETCUSTOMERINFO;
				strRequest += "?shop_id=" + String.valueOf(Global.Cur_ShopId);
				strRequest += "&name=" + (m_CurCustomerDataType == 0 ? EncodeToUTF8(edit_customername.getText().toString()) : "");
				strRequest += "&phone=" + (m_CurCustomerDataType == 1 ? edit_customerphone.getText().toString() : "");
				
				JSONObject response = m_HttpConnUsingJSON.getGetJSONObject(strRequest);
				if (response == null) {
					m_nResponse = ResponseRet.RET_INTERNAL_EXCEPTION;
					return;
				}
				
				m_nResponse = response.getInt(ResponseData.RESPONSE_RET);
				if (m_nResponse == ResponseRet.RET_SUCCESS) {
	            	JSONObject dataObject = response.getJSONObject(ResponseData.RESPONSE_DATA);
					
	            	JSONObject data = dataObject.getJSONObject("data");
	            	
	            	m_CurGetCustomerName = data.getString("name");
	            	m_CurGetCustomerPhone = data.getString("phone");
				}
			}
			else if ( m_reqType == REQ_TYPE.REQ_SALECATALOG )
			{
				m_nResponse = ResponseRet.RET_SUCCESS;
				
				String strRequest = HttpConnUsingJSON.REQ_SALECATALOG;
				
				JSONObject response = m_HttpConnUsingJSON.getPostJSONObject(strRequest);
				if (response == null) {
					m_nResponse = ResponseRet.RET_INTERNAL_EXCEPTION;
					return;
				}
	
				m_nResponse = response.getInt(ResponseData.RESPONSE_RET);
				
				if (m_nResponse == ResponseRet.RET_SUCCESS) {
	            	
				}
	            else if ( m_nResponse == ResponseRet.RET_TICKETNUMUSED )
	            {
	            	JSONObject dataObject = response.getJSONObject(ResponseData.RESPONSE_DATA);
	            	JSONObject ticket_obj = dataObject.getJSONObject("ticket_num");
	            	
	            	m_CurTicketNumber = ticket_obj.getString("data");
	            	m_CurDate = ticket_obj.getString("date");
	            }
			}
		} catch (JSONException e) {
			e.printStackTrace();
			m_nResponse = ResponseRet.RET_JSON_EXCEPTION;
		}
	}
	
	
	public JSONObject makeRequestJSON() throws JSONException {
		
		JSONObject requestObj = new JSONObject();

		requestObj.put("shop_id", String.valueOf(Global.Cur_ShopId));
		requestObj.put("uid", String.valueOf(Global.Cur_UserId));
		requestObj.put("ticketnum", txt_ticketnum.getText().toString());
		requestObj.put("customer_name", edit_customername.getText().toString());
		requestObj.put("customer_phone", edit_customerphone.getText().toString());
		requestObj.put("store_id", m_CurStoreId); //different store with diff catalog??
		requestObj.put("paytype", m_CurPayType);
		requestObj.put("catalogcount", m_SaleCatalogList.size());
		requestObj.put("cataloglist", SaleCatalogListString);
		requestObj.put("sellmoney", m_CurSaveMoney);
		requestObj.put("sellchange", m_CurSmallchangeMoney);
		/*requestObj.put("savemethod", m_CurSaveMethod);
		requestObj.put("savemoney", m_CurSaveMoney);*/
		
		return requestObj;
	}

	@Override
	protected void onActivityResult(int requestCode, int resultCode, Intent data) {
		super.onActivityResult(requestCode, resultCode, data);

		if (resultCode == RESULT_OK)
		{
			switch (requestCode) {
				case ACTIVITY_CODE_SELDEVICE:
				{
					String address = data.getExtras().getString("addr");
					BluetoothDevice device = mBtAdapter.getRemoteDevice(address);
					mConnectedDeviceName = device.getName();

					startProgress(getResources().getString(R.string.STR_NOW_CONNECTING));
					if (device.getBondState() == BluetoothDevice.BOND_NONE) {
						PairDevice(false, device);
					} else if(device.getBondState() == BluetoothDevice.BOND_BONDED) {
						mIsRepair = data.getExtras().getBoolean("repair");
						if (mIsRepair) {
							PairDevice(true, device);
						} else {
							initPrinter(device);
						}
					}
					break;
				}
			}
		}
	}
}
