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
import com.damy.adapters.SaleRejectAdapter;
import com.damy.backend.HttpConnUsingJSON;
import com.damy.backend.LoadResponseThread;
import com.damy.backend.ResponseData;
import com.damy.backend.ResponseRet;
import com.damy.common.Global;
import com.google.zxing.client.android.CaptureActivity;

import android.os.Bundle;
import android.app.Activity;
import android.content.Intent;
import android.graphics.Color;
import android.graphics.drawable.ColorDrawable;
import android.view.Gravity;
import android.view.Menu;
import android.view.View;
import android.view.View.OnClickListener;
import android.view.View.OnFocusChangeListener;

public class SaleRejectActivity extends BaseActivity {
	
	private enum REQ_TYPE{REQ_GETTICKETNUMBER, REQ_GETSTORELIST, REQ_REJECTCATALOG, REQ_GETCATALOGINFOFROMBARCODE, REQ_GETCUSTOMERINFO};
	
	private PopupWindow 						dialog_store;
	private PopupWindow 						dialog_type;
	
	private PopupWindow 						popup_delconfirm;
    private LinearLayout						m_MaskLayer;
    
    private ArrayList<String>					m_typeList = new ArrayList<String>();
    private ArrayList<STStoreInfo>				m_StoreList = new ArrayList<STStoreInfo>();
    private ArrayList<STSaleRejectInfo> 		m_SaleRejectList = new ArrayList<STSaleRejectInfo>();
    
    private AutoSizeTextView					txt_shopname;
    private AutoSizeTextView					txt_ticketnum;
    private AutoSizeTextView					txt_storename;
    private AutoSizeTextView					txt_paytype;
    private AutoSizeEditText					edit_customername;
    private AutoSizeEditText					edit_customerphone;
    
    private SaleRejectAdapter	 				m_SaleRejectAdapter;
    private ListView							m_lvSaleRejectListView;
    
    private int									m_CurStorePos = 0;
    private int									m_CurPayType = 0;
    private String								m_CurTicketNumber = "";
    private int									m_CurLongClickedItem = 0;
    
    private String								SaleRejectListString = "";
    
    private int									m_CurCustomerDataType = 0;
    private String								m_CurGetCustomerName = "";
    private String								m_CurGetCustomerPhone = "";
    
    private REQ_TYPE							m_reqType;

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
		setContentView(R.layout.activity_sale_reject);
		
		ResolutionSet._instance.iterateChild(findViewById(R.id.fl_salereject));
		
		Global.SaleReject_isSelected = false;
		
		initControls();
		setDialogTypeAdapter();
		readContents();
	}
	
	@Override
	protected void onResume()
	{
		super.onResume();			
		setSaleRejectAdapter();
	}
	
	private void initControls()
	{
		FrameLayout fl_backbtn = (FrameLayout)findViewById(R.id.fl_salereject_backbtn);
		FrameLayout fl_homebtn = (FrameLayout)findViewById(R.id.fl_salereject_homebtn);
		FrameLayout fl_addbtn = (FrameLayout)findViewById(R.id.fl_salecatalog_addbtn);
		FrameLayout fl_okbtn = (FrameLayout)findViewById(R.id.fl_salereject_savebtn);
		FrameLayout fl_cancelbtn = (FrameLayout)findViewById(R.id.fl_salereject_cancelbtn);
		FrameLayout fl_printbtn = (FrameLayout)findViewById(R.id.fl_salereject_printbtn);
		
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
        		onClickSave();
        	}
        });
		
		fl_cancelbtn.setOnClickListener(new OnClickListener() {
        	public void onClick(View v) {
        		onClickCancel();
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

		txt_shopname = (AutoSizeTextView)findViewById(R.id.txt_salereject_shopname);
		txt_ticketnum = (AutoSizeTextView)findViewById(R.id.txt_salereject_ticketnum);
		txt_storename = (AutoSizeTextView)findViewById(R.id.txt_salereject_storename);
		txt_paytype = (AutoSizeTextView)findViewById(R.id.txt_salereject_paytype);
		
		edit_customername = (AutoSizeEditText)findViewById(R.id.edit_salereject_customername);
		edit_customerphone = (AutoSizeEditText)findViewById(R.id.edit_salereject_customerphone);
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
		
		LinearLayout ll_storeselect = (LinearLayout)findViewById(R.id.ll_salereject_storesel);
		ll_storeselect.setOnClickListener(new OnClickListener() {
        	public void onClick(View v) {
        		onClickStoreSelect();
        	}
        });
		
		LinearLayout ll_paytypeelect = (LinearLayout)findViewById(R.id.ll_salereject_paytypesel);
		ll_paytypeelect.setOnClickListener(new OnClickListener() {
        	public void onClick(View v) {
        		onClickPaytypeSelect();
        	}
        });
		
		m_MaskLayer = (LinearLayout)findViewById(R.id.ll_salereject_masklayer);
		m_MaskLayer.setVisibility(View.INVISIBLE);
		
		m_lvSaleRejectListView = (ListView)findViewById(R.id.anSaleRejectContentView);
		m_SaleRejectList = new ArrayList<STSaleRejectInfo>();
		
        m_SaleRejectAdapter = new SaleRejectAdapter(SaleRejectActivity.this, m_SaleRejectList);
        m_lvSaleRejectListView.setAdapter(m_SaleRejectAdapter);
        
        m_lvSaleRejectListView.setCacheColorHint(Color.TRANSPARENT);
        m_lvSaleRejectListView.setDividerHeight(0);
        m_lvSaleRejectListView.setDrawSelectorOnTop(true);
        
        m_lvSaleRejectListView.setOnItemLongClickListener(new AdapterView.OnItemLongClickListener() {
			public boolean onItemLongClick(AdapterView<?> parent, View view, int position, long id) {
				onLongClickItem(parent, position);
				return true;
        	}
		});
	}
	
	private void readContents()
	{
		txt_shopname.setText(Global.Cur_ShopName);
		txt_paytype.setText(getResources().getString(R.string.common_actual));
		
		m_reqType = REQ_TYPE.REQ_GETTICKETNUMBER;
		new LoadResponseThread(SaleRejectActivity.this).start();
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
		Global.SaleReject_isSelected = false;
		Intent salecatalog_add_activity = new Intent(this, SaleRejectAddActivity.class);
		startActivity(salecatalog_add_activity);
	}
	
	private void onClickSave()
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
		
		int cnt = m_SaleRejectList.size();
		SaleRejectListString = "";
		
		for ( int i = 0; i < cnt; i++ )
		{
			STSaleRejectInfo item = m_SaleRejectList.get(i);
			
			SaleRejectListString += String.valueOf(item.catalog_id) + "," + String.valueOf(item.standard_id) + "," + String.valueOf(item.largenumber) + "," + String.valueOf(item.oneprice) + "," + String.valueOf(item.quantity) + "@";
		}
		
		m_reqType = REQ_TYPE.REQ_REJECTCATALOG;
		new LoadResponseThread(SaleRejectActivity.this).start();
	}
	
	private void onClickCancel()
	{
		m_SaleRejectList.clear();
		m_SaleRejectAdapter.notifyDataSetChanged();
		
		txt_shopname.setText(Global.Cur_ShopName);
		txt_paytype.setText(getResources().getString(R.string.common_actual));
		
		edit_customername.setText("");
		edit_customerphone.setText("");
		
		m_reqType = REQ_TYPE.REQ_GETTICKETNUMBER;
		new LoadResponseThread(SaleRejectActivity.this).start();
	}
	
	private void onGetCustomerPhone()
	{
		if ( edit_customername.getText().toString().length() == 0 )
			return;
		
		m_CurCustomerDataType = 0;
		m_reqType = REQ_TYPE.REQ_GETCUSTOMERINFO;
		new LoadResponseThread(SaleRejectActivity.this).start();
	}
	
	private void onGetCustomerName()
	{
		if ( edit_customerphone.getText().toString().length() == 0 )
			return;
		
		m_CurCustomerDataType = 1;
		m_reqType = REQ_TYPE.REQ_GETCUSTOMERINFO;
		new LoadResponseThread(SaleRejectActivity.this).start();
	}
	
	private void onClickStoreSelect()
	{
		if ( m_StoreList.size() > 0 )
		{
			m_MaskLayer.setVisibility(View.VISIBLE);
			dialog_store.showAtLocation(findViewById(R.id.ll_salereject_masklayer), Gravity.CENTER_HORIZONTAL | Gravity.CENTER_VERTICAL, 0, 0);
		}
	}
	
	private void onClickPaytypeSelect()
	{
		m_MaskLayer.setVisibility(View.VISIBLE);
		dialog_type.showAtLocation(findViewById(R.id.ll_salereject_masklayer), Gravity.CENTER_HORIZONTAL | Gravity.CENTER_VERTICAL, 0, 0);
	}
	
	private void setSaleRejectAdapter()
	{
		STSaleRejectInfo newitem = new STSaleRejectInfo();
		if(Global.SaleReject_isSelected == true)
		{
			newitem.catalog_id = Global.SaleReject_SelectItem.catalog_id;
			newitem.catalog_name = Global.SaleReject_SelectItem.catalog_name;
			newitem.standard_id = Global.SaleReject_SelectItem.standard_id;
			newitem.standard_name = Global.SaleReject_SelectItem.standard_name;
			newitem.largenumber = Global.SaleReject_SelectItem.largenumber;
			newitem.oneprice = Global.SaleReject_SelectItem.oneprice;
			newitem.quantity = Global.SaleReject_SelectItem.quantity;
			newitem.totalprice = Global.SaleReject_SelectItem.totalprice;
					
			m_SaleRejectList.add(newitem);
			m_SaleRejectAdapter.notifyDataSetChanged();

			Global.SaleReject_isSelected = false;
		}
	}
	
	private void onLongClickItem(View view, int position)
	{
		m_CurLongClickedItem = position;
		
		m_MaskLayer.setVisibility(View.VISIBLE);
		
		View popupview = View.inflate(this, R.layout.dialog_delconfirm, null);
		ResolutionSet._instance.iterateChild(popupview);
		popup_delconfirm = new PopupWindow(popupview, R.dimen.common_delconfirm_dialog_width, R.dimen.common_delconfirm_dialog_height,true);
		popup_delconfirm.setAnimationStyle(-1);
		
		popup_delconfirm.showAtLocation(findViewById(R.id.ll_salereject_masklayer), Gravity.CENTER_HORIZONTAL | Gravity.CENTER_VERTICAL, 0, 0);
		
		AutoSizeTextView txt_delconfirm_msg = (AutoSizeTextView)popupview.findViewById(R.id.txt_dialog_delconfirm_msg);
		txt_delconfirm_msg.setText(getResources().getString(R.string.confirm_del_salereject));
		
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
		
		m_SaleRejectList.remove(m_CurLongClickedItem);
		m_SaleRejectAdapter.notifyDataSetChanged();
	}
	
	private void onClickDelConfirmCancel()
	{
		m_MaskLayer.setVisibility(View.INVISIBLE);
		popup_delconfirm.dismiss();
	}
	
	private void setDialogStoreAdapter()
	{
		View popupview = View.inflate(this, R.layout.dialog_select, null);
		ResolutionSet._instance.iterateChild(popupview);
		
		AutoSizeTextView txt_title = (AutoSizeTextView)popupview.findViewById(R.id.txt_dialog_title);
		txt_title.setText(getResources().getString(R.string.dialog_select_store));

        ImageView img_cancel = (ImageView) popupview.findViewById(R.id.img_dialog_cancel);
        img_cancel.setOnClickListener(new OnClickListener() {
            @Override
            public void onClick(View v) {
                if (dialog_store != null && dialog_store.isShowing() )
                {
                    m_MaskLayer.setVisibility(View.INVISIBLE);
                    dialog_store.dismiss();
                }
            }
        });
		
		ArrayList<String> arGeneral = new ArrayList<String>();
		
		int cnt = m_StoreList.size();
		for ( int i = 0; i < cnt; i++ )
			arGeneral.add(m_StoreList.get(i).name);
		
		DialogSelectAdapter Adapter = new DialogSelectAdapter(this, arGeneral);
		
		ListView list = (ListView)popupview.findViewById(R.id.lv_dialog_listview);
		list.setAdapter(Adapter);
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
			m_CurStorePos = 0;
			txt_storename.setText(m_StoreList.get(m_CurStorePos).name);
		}
		else
			txt_storename.setText("");
	}
	
	private void onClickStoreItem(int pos)
	{
		m_MaskLayer.setVisibility(View.INVISIBLE);
		dialog_store.dismiss();

		txt_storename.setText(m_StoreList.get(pos).name);
		m_CurStorePos = pos;
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
	
	private void onClickTypeItem(int pos)
	{
		m_MaskLayer.setVisibility(View.INVISIBLE);
		dialog_type.dismiss();
		
		txt_paytype.setText(m_typeList.get(pos));
		if ( pos == 0 )
			m_CurPayType = 0;
		else if ( pos == 1 )
			m_CurPayType = 1;
		else if ( pos == 2 )
			m_CurPayType = 2;
	}
	
	public void refreshUI() {
		super.refreshUI();
		
		if ( m_reqType == REQ_TYPE.REQ_GETTICKETNUMBER )
		{
			txt_ticketnum.setText(m_CurTicketNumber);
			
			m_reqType = REQ_TYPE.REQ_GETSTORELIST;
			new LoadResponseThread(SaleRejectActivity.this).start();
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
		else if ( m_reqType == REQ_TYPE.REQ_REJECTCATALOG )
		{
			if (m_nResponse == ResponseRet.RET_SUCCESS)
			{
				showToastMessage(getResources().getString(R.string.common_success));
				onClickCancel();
			}
			else if ( m_nResponse == ResponseRet.RET_TICKETNUMUSED )
			{
				txt_ticketnum.setText(m_CurTicketNumber);
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
				strRequest += "&type=3";
				
				JSONObject response = m_HttpConnUsingJSON.getGetJSONObject(strRequest);
				if (response == null) {
					m_nResponse = ResponseRet.RET_INTERNAL_EXCEPTION;
					return;
				}
				
				m_nResponse = response.getInt(ResponseData.RESPONSE_RET);
				if (m_nResponse == ResponseRet.RET_SUCCESS) {
					
					JSONObject dataObject = response.getJSONObject(ResponseData.RESPONSE_DATA);
					
					m_CurTicketNumber = dataObject.getString("data");
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
					
					m_StoreList.clear();
					
					for (int i = 0; i < count; i++)
					{
						STStoreInfo itemInfo = new STStoreInfo();
						JSONObject tmpObj = dataList.getJSONObject(i);
						
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
			else if ( m_reqType == REQ_TYPE.REQ_REJECTCATALOG )
			{
				m_nResponse = ResponseRet.RET_SUCCESS;
				
				String strRequest = HttpConnUsingJSON.REQ_REJECTCATALOG;
				
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
		requestObj.put("store_id", String.valueOf(m_StoreList.get(m_CurStorePos).id));
		requestObj.put("customer_name", edit_customername.getText().toString());
		requestObj.put("customer_phone", edit_customerphone.getText().toString());
		requestObj.put("paytype", String.valueOf(m_CurPayType));
		requestObj.put("catalogcount", String.valueOf(m_SaleRejectList.size()));
		requestObj.put("cataloglist", SaleRejectListString);
		
		return requestObj;
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
	public STSaleRejectPrintInfo getSaleRejectPrintInfo()
	{
		STSaleRejectPrintInfo printInfo = new STSaleRejectPrintInfo();

		printInfo.store_name = txt_storename.getText().toString();      //
		printInfo.shop_name = txt_shopname.getText().toString();           //
		printInfo.ticket_name = txt_ticketnum.getText().toString();                                //
		printInfo.customer_name = edit_customername.getText().toString();                                    //
		printInfo.phone_number = edit_customerphone.getText().toString();

		ArrayList<STSaleRejectInfo> _reject_info = new ArrayList<STSaleRejectInfo>();
		int i, cnt = m_SaleRejectList.size();
		for ( i = 0; i < cnt; i++ )
		{
			STSaleRejectInfo item = m_SaleRejectList.get(i);
			_reject_info.add(item);
		}
		printInfo.reject_info = _reject_info;

		return printInfo;
	}

	private void onClickPrint()
	{
		if (mConnectDevice)
		{
			int nResult = PrintUtils.printSaleRejectInfo(getResources(), getSaleRejectPrintInfo(), mPrinter);
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
			Intent intent = new Intent(SaleRejectActivity.this, SelectDeviceActivity.class);
			intent.putExtra("mac_addr", mMacAddr);
			intent.putExtra("name", mConnectedDeviceName);
			startActivityForResult(intent, ACTIVITY_CODE_SELDEVICE);
		}
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
