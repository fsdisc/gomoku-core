/*
 *  Gomoku Core
 * 
 *  Copyright (c) 2011 Tran Dinh Thoai <dthoai@yahoo.com>
 *
 * This library is free software; you can redistribute it and/or
 * modify it under the terms of the GNU General Public License
 * version 3.
 *
 * This library is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
 * General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with this library; if not, write to the Free Software
 * Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
 */

package com.bhivef.gomoku.core;

import java.io.FileInputStream;
import java.io.FileOutputStream;
import java.security.GeneralSecurityException;
import java.text.DateFormat;
import java.text.SimpleDateFormat;
import java.util.Calendar;
import java.util.Date;
import java.util.Properties;

import javax.crypto.Cipher;
import javax.crypto.spec.SecretKeySpec;

public class Config {

    public static final boolean BOOLEAN_DEFAULT = false;
    public static final double DOUBLE_DEFAULT = 0.0;
    public static final float FLOAT_DEFAULT = 0.0f;
    public static final int INT_DEFAULT = 0;
    public static final long LONG_DEFAULT = 0L;
    public static final byte BYTE_DEFAULT = 0;
    public static final String STRING_DEFAULT = "";
    public static final String PASSWORD_DEFAULT = "";
	
    public static final String TRUE = "true";
    public static final String FALSE = "false";

    public static final DateFormat DATE_FORMATTER = new SimpleDateFormat("yyyy-MM-dd'T'HH:mm:ss.SSSZ");
    
    public static final String SERVER_URL = "server.url";
    public static final String SERVER_USERNAME = "server.username";
    public static final String SERVER_PASSWORD = "server.password";
    public static final String SERVER_EXTENSION = "server.extension";
    public static final String SEARCH_DEPTH = "search.depth";
    public static final String FIRST_TYPE = "type.first";
    public static final String SECOND_TYPE = "type.second";
    public static final String BOARD_WIDTH = "board.width";
    public static final String BOARD_HEIGHT = "board.height";
    public static final String CURRENT_GAME = "game.current";

	private final static String KEY = "968AD25B96915DD9267110A56E37D838";
	
    private Properties properties;

    public Config() {
    	this.properties = new Properties();
    }
    
    public boolean getBoolean(String name) {
        String value = properties.getProperty(name);
        if (value == null || value.length() == 0) return BOOLEAN_DEFAULT;
        if (value.equals(Config.TRUE)) return true;
        return false;
    }
    
    public double getDouble(String name) {
        String value = properties.getProperty(name);
        if (value == null || value.length() == 0) return DOUBLE_DEFAULT;
        double ival = DOUBLE_DEFAULT;
        try {
            ival = new Double(value).doubleValue();
        } catch (Exception e) {
            ival = DOUBLE_DEFAULT;
        }
        return ival;
    }

    public float getFloat(String name) {
        String value = properties.getProperty(name);
        if (value == null || value.length() == 0) return FLOAT_DEFAULT;
        float ival = FLOAT_DEFAULT;
        try {
            ival = new Float(value).floatValue();
        } catch (Exception e) {
            ival = FLOAT_DEFAULT;
        }
        return ival;
    }

    public int getInt(String name) {
        String value = properties.getProperty(name);
        if (value == null || value.length() == 0) return INT_DEFAULT;
        int ival = INT_DEFAULT;
        try {
            ival = Integer.parseInt(value);
        } catch (Exception e) {
            ival = INT_DEFAULT;
        }
        return ival;
    }

    public byte getByte(String name) {
        String value = properties.getProperty(name);
        if (value == null || value.length() == 0) return BYTE_DEFAULT;
        byte ival = BYTE_DEFAULT;
        try {
        	ival = Byte.parseByte(value);
        } catch (Exception e) {
            ival = BYTE_DEFAULT;
        }
        return ival;
    }
    
    public long getLong(String name) {
        String value = properties.getProperty(name);
        if (value == null || value.length() == 0) return LONG_DEFAULT;
        long ival = LONG_DEFAULT;
        try {
            ival = Long.parseLong(value);
        } catch (Exception e) {
            ival = LONG_DEFAULT;
        }
        return ival;
    }
    
    public String getString(String name) {
        String value = properties.getProperty(name);
        if (value == null) return STRING_DEFAULT;
        return value;
    }
    
    public String getPassword(String name) {
    	String src = getString(name);
    	String tag = PASSWORD_DEFAULT;
    	try {
    		tag = decrypt(src, KEY);
    	} catch (Exception e) {
    		tag = PASSWORD_DEFAULT;
    	}
    	return tag;
    }
    
    public Date getDate(String name) {
        String value = properties.getProperty(name);
        if (value == null || value.length() == 0) return Calendar.getInstance().getTime();
        Date dval = Calendar.getInstance().getTime();
        try {
            dval = DATE_FORMATTER.parse(value);
        } catch (Exception e) {
            dval = Calendar.getInstance().getTime();
        }
        return dval;
    }
    
    public void setValue(String name, double value) {
        double oldValue = getDouble(name);
        if (oldValue != value) {
        	properties.put(name, Double.toString(value));
        }
    }

    public void setValue(String name, float value) {
        float oldValue = getFloat(name);
        if (oldValue != value) {
        	properties.put(name, Float.toString(value));
        }
    }

    public void setValue(String name, int value) {
        int oldValue = getInt(name);
        if (oldValue != value) {
        	properties.put(name, Integer.toString(value));
        }
    }

    public void setValue(String name, byte value) {
        int oldValue = getByte(name);
        if (oldValue != value) {
        	properties.put(name, Byte.toString(value));
        }
    }
    
    public void setValue(String name, long value) {
        long oldValue = getLong(name);
        if (oldValue != value) {
        	properties.put(name, Long.toString(value));
        }
    }

    public void setValue(String name, String value) {
        String oldValue = getString(name);
        if (oldValue == null || !oldValue.equals(value)) {
            if (value != null) {
            	properties.put(name, value);
            }
        }
    }

    public void setPassword(String name, String value) {
    	String oldValue = getPassword(name);
    	if (!oldValue.equals(value)) {
    		if (value != null) {
    			try {
                	properties.put(name, encrypt(value, KEY));
    			} catch (Exception e) {
    			}
    		}
    	}
    }
    
    public void setValue(String name, boolean value) {
        boolean oldValue = getBoolean(name);
        if (oldValue != value) {
        	properties.put(name, value == true ? Config.TRUE : Config.FALSE);
        }
    }

    public void setValue(String name, Date value) {
        Date oldValue = getDate(name);
        if (oldValue != value) {
        	properties.put(name, DATE_FORMATTER.format(value));
        }
    }

    public void save(String filename) {
        try {
            properties.store(new FileOutputStream(filename), "Gomoku Config");
        } catch (Exception e) {
        }
    }
    
    public void load(String filename) {
    	try {
    		properties.load(new FileInputStream(filename));
    	} catch (Exception e) {
    	}
    }
    
	public static String encrypt(String value, String key) throws GeneralSecurityException {
		SecretKeySpec sks = new SecretKeySpec(hexStringToByteArray(key), "AES");
		Cipher cipher = Cipher.getInstance("AES");
		cipher.init(Cipher.ENCRYPT_MODE, sks, cipher.getParameters());
		byte[] encrypted = cipher.doFinal(value.getBytes());
		return byteArrayToHexString(encrypted);
	}
	
	public static String decrypt(String message, String key) throws GeneralSecurityException {
		SecretKeySpec sks = new SecretKeySpec(hexStringToByteArray(key), "AES");
		Cipher cipher = Cipher.getInstance("AES");
		cipher.init(Cipher.DECRYPT_MODE, sks);
		byte[] decrypted = cipher.doFinal(hexStringToByteArray(message));
		return new String(decrypted);
	}
    
	private static String byteArrayToHexString(byte[] b){
		StringBuffer sb = new StringBuffer(b.length * 2);
		for (int i = 0; i < b.length; i++){
			int v = b[i] & 0xff;
			if (v < 16) {
				sb.append('0');
			}
			sb.append(Integer.toHexString(v));
		}
		return sb.toString().toUpperCase();
	}

	private static byte[] hexStringToByteArray(String s) {
		byte[] b = new byte[s.length() / 2];
		for (int i = 0; i < b.length; i++){
			int index = i * 2;
			int v = Integer.parseInt(s.substring(index, index + 2), 16);
			b[i] = (byte)v;
		}
		return b;
	}
    
}
