package com.damy.nongyao;

import android.app.AlertDialog;
import android.bluetooth.BluetoothAdapter;
import android.bluetooth.BluetoothDevice;
import android.content.*;
import android.graphics.Color;
import android.graphics.Point;
import android.graphics.Typeface;
import android.os.Bundle;
import android.view.View;
import android.view.ViewGroup;
import android.view.ViewTreeObserver;
import android.widget.*;
import com.damy.common.Global;
import com.damy.datatypes.STPrinter;
import com.damy.Utils.ResolutionSet;
import java.util.ArrayList;
import java.util.Set;
import java.util.TimerTask;

/**
 * Created with IntelliJ IDEA.
 * User: KHM
 * Date: 14-3-24
 * Time: 上午1:01
 * To change this template use File | Settings | File Templates.
 */
public class SelectDeviceActivity extends BaseActivity {
	private ImageButton mBtnBack = null, mBtnSearch = null;
	private LinearLayout mListLayout = null;
	private ArrayList<STPrinter> mArrPrinters = new ArrayList<STPrinter>();
	private ArrayList<TextView> mArrMacViews = new ArrayList<TextView>();
	private ArrayList<TextView> mArrNameViews = new ArrayList<TextView>();

//	private ImageButton mBtnRemin = null, mBtnZhenDa = null, mBtn58mm = null, mBtn80mm = null;
//	private ImageView mImgRemin = null, mImgZhenDa = null, mImg58mm = null, mImg80mm = null;

	private String mMacAddr = "", mName = "";
	private BluetoothAdapter mBtAdapter = null;
	private boolean mRepair = false;
//	private boolean mIsThermal = true, mIs58mm = true;

	@Override
	protected void onCreate(Bundle savedInstanceState) {
		super.onCreate(savedInstanceState);    //To change body of overridden methods use File | Settings | File Templates.
		setContentView(R.layout.activity_printerlist);

		initResolution();
		loadExtras();
		initControls();
	}

	@Override
	protected void onStop() {
		super.onPause();
		// Unregister broadcast listeners
		this.unregisterReceiver(mReceiver);
	}

	@Override
	protected void onResume() {
		super.onResume();
		// Register for broadcasts when a device is discovered and discovery has finished
		IntentFilter filter = new IntentFilter(BluetoothDevice.ACTION_FOUND);
		filter.addAction(BluetoothAdapter.ACTION_DISCOVERY_FINISHED);
		registerReceiver(mReceiver, filter);
	}



	private void loadExtras()
	{
		mMacAddr = getIntent().getStringExtra("mac_addr");
		mName = getIntent().getStringExtra("name");
	}

	private void initResolution() {
		RelativeLayout parent_layout = (RelativeLayout)findViewById(R.id.parent_layout);
		parent_layout.getViewTreeObserver().addOnGlobalLayoutListener (new ViewTreeObserver.OnGlobalLayoutListener() {
			@Override
			public void onGlobalLayout() {
				Point ptTemp = Global.getScreenSize(getApplicationContext());
				boolean bNeedUpdate = false;
				if (mScrSize.x == 0 && mScrSize.y == 0)
				{
					mScrSize = ptTemp;
					bNeedUpdate = true;
				}
				else if (mScrSize.x != ptTemp.x || mScrSize.y != ptTemp.y)
				{
					mScrSize = ptTemp;
					bNeedUpdate = true;
				}

				if (bNeedUpdate)
				{
					runOnUiThread(new Runnable() {
						@Override
						public void run() {
							ResolutionSet._instance.iterateChild(findViewById(R.id.parent_layout));
						}
					});
				}
			}
		});
	}

	private void initControls()
	{
		mBtnBack = (ImageButton)findViewById(R.id.btn_back);
		mBtnSearch = (ImageButton)findViewById(R.id.btn_refresh);
		mListLayout = (LinearLayout)findViewById(R.id.list_layout);

		mBtnBack.setOnClickListener(new View.OnClickListener() {
			@Override
			public void onClick(View v) {
				finish();
			}
		});
		mBtnSearch.setOnClickListener(new View.OnClickListener() {
			@Override
			public void onClick(View v) {
				onSearch();
			}
		});

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

		Set<BluetoothDevice> pairedDevices = mBtAdapter.getBondedDevices();

		// If there are paired devices, add each one to the ArrayAdapter
		if (pairedDevices.size() > 0) {
			for (BluetoothDevice device : pairedDevices) {
				mArrPrinters.add(new STPrinter(device.getName(), device.getAddress()));
			}
			updateList();
			selectDevice(mMacAddr);
		}
	}

	private void updateList()
	{
		mListLayout.removeAllViews();

		mArrNameViews.clear();
		mArrMacViews.clear();

		for (int i = 0; i < mArrPrinters.size(); i++)
		{
			STPrinter printer = mArrPrinters.get(i);

			RelativeLayout itemLayout = (RelativeLayout)getLayoutInflater().inflate(R.layout.printer_item, null);
			RelativeLayout.LayoutParams layoutParams = new RelativeLayout.LayoutParams(ViewGroup.LayoutParams.MATCH_PARENT, 90);
			itemLayout.setLayoutParams(layoutParams);

			TextView txtName = (TextView)itemLayout.findViewById(R.id.txt_name);
			TextView txtMac = (TextView)itemLayout.findViewById(R.id.txt_mac);
			ImageButton btnItem = (ImageButton)itemLayout.findViewById(R.id.btn_item);

			mArrNameViews.add(txtName);
			mArrMacViews.add(txtMac);

			txtName.setText(printer.name);
			txtMac.setText(printer.mac_addr);
			btnItem.setTag(printer.mac_addr);
			btnItem.setOnClickListener(new View.OnClickListener() {
				@Override
				public void onClick(View v) {
					String szMac = (String) v.getTag();
					selectDevice(szMac);
					finishWithResult();
				}
			});
			btnItem.setLongClickable(true);
			btnItem.setOnLongClickListener(new View.OnLongClickListener() {
				@Override
				public boolean onLongClick(View v) {
					AlertDialog.Builder builder = new AlertDialog.Builder(SelectDeviceActivity.this);
					builder = new AlertDialog.Builder(SelectDeviceActivity.this);
					builder.setMessage(getResources().getString(R.string.STR_CONFIRM_REPAIR));
					builder.setCancelable(false);
					builder.setPositiveButton(getResources().getString(R.string.common_ok), new DialogInterface.OnClickListener() {
						@Override
						public void onClick(DialogInterface dialog, int which) {
							mRepair = true;
							finishWithResult();
						}
					});
					builder.setNegativeButton(getResources().getString(R.string.common_cancel), null);
					builder.show();
					return true;
				}
			});

			ResolutionSet._instance.iterateChild(itemLayout);

			mListLayout.addView(itemLayout);
		}
	}

	private void selectDevice(String szMac)
	{
		mMacAddr = szMac;

		for (int i = 0; i < mArrPrinters.size(); i++)
		{
			if (mArrPrinters.get(i).mac_addr.equals(mMacAddr))
			{
				mArrNameViews.get(i).setTextColor(Color.GREEN);
				mArrNameViews.get(i).setTypeface(Typeface.DEFAULT_BOLD);

				mArrMacViews.get(i).setTextColor(Color.GREEN);
				mArrMacViews.get(i).setTypeface(Typeface.DEFAULT_BOLD);

				mName = mArrPrinters.get(i).name;
			}
			else
			{
				mArrNameViews.get(i).setTextColor(Color.WHITE);
				mArrNameViews.get(i).setTypeface(Typeface.DEFAULT);

				mArrMacViews.get(i).setTextColor(Color.WHITE);
				mArrMacViews.get(i).setTypeface(Typeface.DEFAULT);
			}
		}
	}

	private final BroadcastReceiver mReceiver = new BroadcastReceiver() {
		@Override
		public void onReceive(Context context, Intent intent) {
			String action = intent.getAction();

			// When discovery finds a device
			if (BluetoothDevice.ACTION_FOUND.equals(action)) {
				// Get the BluetoothDevice object from the Intent
				BluetoothDevice device = intent.getParcelableExtra(BluetoothDevice.EXTRA_DEVICE);
				// If it's already paired, skip it, because it's been listed already
				String itemName = device.getName();
				String itemAddr = device.getAddress();
				removeDeviceWithName(itemName);
				removeDeviceWithAddr(itemAddr);
				addDevice(new STPrinter(itemName, itemAddr));
				updateList();
				selectDevice(mMacAddr);
			} else if (BluetoothAdapter.ACTION_DISCOVERY_FINISHED.equals(action)) {
				stopProgress();
			}
		}
	};

	private void removeDeviceWithName(String szName)
	{
		for (int i = 0; i < mArrPrinters.size(); i++)
		{
			if (mArrPrinters.get(i).name.equals(szName))
			{
				mArrPrinters.remove(i);
				break;
			}
		}
	}

	private void removeDeviceWithAddr(String szAddr)
	{
		for (int i = 0; i < mArrPrinters.size(); i++)
		{
			if (mArrPrinters.get(i).mac_addr.equals(szAddr))
			{
				mArrPrinters.remove(i);
				break;
			}
		}
	}

	private void addDevice(STPrinter printer)
	{
		for (int i = 0; i < mArrPrinters.size(); i++)
		{
			if (mArrPrinters.get(i).name.equals(printer.name) ||
					mArrPrinters.get(i).mac_addr.equals(printer.mac_addr))
			{
				mArrPrinters.remove(i);
				break;
			}
		}

		mArrPrinters.add(printer);
	}



	TimerTask search_timertask = new TimerTask() {
		@Override
		public void run() {
			runOnUiThread(new Runnable() {
				@Override
				public void run() {
					mMacAddr = "";

					startProgress(getResources().getString(R.string.STR_NOW_SEARCHING));

					if (mBtAdapter.isDiscovering()) {
						mBtAdapter.cancelDiscovery();
					}

					mListLayout.removeAllViews();
					mArrPrinters.clear();
					mArrNameViews.clear();
					mArrMacViews.clear();

					mBtAdapter.startDiscovery();
				}
			});
		}
	};

	private void onSearch()
	{
		if (mBtAdapter == null)
			return;

		search_timertask.run();
	}

	private void finishWithResult()
	{
		Intent intent = new Intent();
		intent.putExtra("addr", mMacAddr);
		intent.putExtra("name", mName);
		intent.putExtra("repair", mRepair);
		setResult(RESULT_OK, intent);
		finish();
	}
}
