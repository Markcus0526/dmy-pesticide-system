package com.damy.datatypes;

import org.json.JSONObject;

/**
 * Created with IntelliJ IDEA.
 * User: KHM
 * Date: 14-3-24
 * Time: 上午12:48
 * To change this template use File | Settings | File Templates.
 */
public class STPrinter {
	public String name = "";
	public String mac_addr = "";
	public boolean selected = false;

	public STPrinter(String szName, String szAddr)
	{
		name = szName;
		mac_addr = szAddr;
		selected = false;
	}

	public STPrinter()
	{
		name = "";
		mac_addr = "";
		selected = false;
	}

	public static STPrinter DecodeFromJSON(JSONObject jsonObj)
	{
		STPrinter printer = new STPrinter();

		try { printer.name = jsonObj.getString("name"); } catch (Exception ex) { ex.printStackTrace(); }
		try { printer.mac_addr = jsonObj.getString("mac_addr"); } catch (Exception ex) { ex.printStackTrace(); }
		try { printer.selected = jsonObj.getBoolean("selected"); } catch (Exception ex) { ex.printStackTrace(); }

		return printer;
	}

	public static JSONObject EncodeToJSON(STPrinter printer)
	{
		JSONObject jsonResult = new JSONObject();

		try { jsonResult.put("name", printer.name); } catch (Exception ex) { ex.printStackTrace(); }
		try { jsonResult.put("mac_addr", printer.mac_addr); } catch (Exception ex) { ex.printStackTrace(); }
		try { jsonResult.put("selected", printer.selected); } catch (Exception ex) { ex.printStackTrace(); }

		return jsonResult;
	}
}
