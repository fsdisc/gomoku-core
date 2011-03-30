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

using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace com.bhivef.gomoku.core
{
    public class Config
    {
        public const bool BOOLEAN_DEFAULT = false;
        public const double DOUBLE_DEFAULT = 0;
        public const float FLOAT_DEFAULT = 0;
        public const int INT_DEFAULT = 0;
        public const long LONG_DEFAULT = 0;
        public const byte BYTE_DEFAULT = 0;
        public const String STRING_DEFAULT = "";
        public const String PASSWORD_DEFAULT = "";
	
        public const String TRUE = "true";
        public const String FALSE = "false";

        public const String SERVER_URL = "server.url";
        public const String SERVER_USERNAME = "server.username";
        public const String SERVER_PASSWORD = "server.password";
        public const String SERVER_EXTENSION = "server.extension";
        public const String SEARCH_DEPTH = "search.depth";
        public const String FIRST_TYPE = "type.first";
        public const String SECOND_TYPE = "type.second";
        public const String BOARD_WIDTH = "board.width";
        public const String BOARD_HEIGHT = "board.height";
        public const String CURRENT_GAME = "game.current";

	    private const String KEY = "968AD25B";
	
        private Dictionary<String, String> properties;

        public Config() 
        {
    	    this.properties = new Dictionary<String, String>();
        }
    
        private String getProperty(String name) 
        {
            String value = "";
            if (!properties.TryGetValue(name, out value)) 
            {
                value = "";
            }
            return value;
        }

        private void setProperty(String name, String value)
        {
            if (properties.ContainsKey(name))
            {
                properties[name] = value;
            }
            else 
            {
                properties.Add(name, value);
            }
        }

        public bool getBoolean(String name) 
        {
            String value = getProperty(name);
            if (value.Length == 0) return BOOLEAN_DEFAULT;
            if (value.Equals(Config.TRUE)) return true;
            return false;
        }
    
        public double getDouble(String name) 
        {
            String value = getProperty(name);
            if (value.Length == 0) return DOUBLE_DEFAULT;
            double ival = DOUBLE_DEFAULT;
            if (!double.TryParse(value, out ival))
            {
                ival = DOUBLE_DEFAULT;
            }
            return ival;
        }

        public float getFloat(String name) 
        {
            String value = getProperty(name);
            if (value.Length == 0) return FLOAT_DEFAULT;
            float ival = FLOAT_DEFAULT;
            if (!float.TryParse(value, out ival)) 
            {
                ival = FLOAT_DEFAULT;
            }
            return ival;
        }

        public int getInt(String name) 
        {
            String value = getProperty(name);
            if (value.Length == 0) return INT_DEFAULT;
            int ival = INT_DEFAULT;
            if (!int.TryParse(value, out ival))
            {
                ival = INT_DEFAULT;
            }
            return ival;
        }

        public byte getByte(String name) 
        {
            String value = getProperty(name);
            if (value.Length == 0) return BYTE_DEFAULT;
            byte ival = BYTE_DEFAULT;
            if (!byte.TryParse(value, out ival))
            {
                ival = BYTE_DEFAULT;
            }
            return ival;
        }
    
        public long getLong(String name) 
        {
            String value = getProperty(name);
            if (value.Length == 0) return LONG_DEFAULT;
            long ival = LONG_DEFAULT;
            if (!long.TryParse(value, out ival))
            {
                ival = LONG_DEFAULT;
            }
            return ival;
        }
    
        public String getString(String name) 
        {
            String value = getProperty(name);
            if (value.Length == 0) return STRING_DEFAULT;
            return value;
        }
    
        public String getPassword(String name) 
        {
    	    String src = getString(name);
    	    String tag = PASSWORD_DEFAULT;
    	    try 
            {
    		    tag = decrypt(src, KEY);
    	    }
            catch
            {
    		    tag = PASSWORD_DEFAULT;
    	    }
    	    return tag;
        }
    
        public DateTime getDate(String name) 
        {
            DateTime ival = DateTime.Now;
            String value = getProperty(name);
            if (value.Length == 0) return ival;
            if (!DateTime.TryParse(value, out ival)) 
            {
                ival = DateTime.Now;
            }
            return ival;
        }
    
        public void setValue(String name, double value) 
        {
            double oldValue = getDouble(name);
            if (oldValue != value) 
            {
        	    setProperty(name, value.ToString());
            }
        }

        public void setValue(String name, float value) 
        {
            float oldValue = getFloat(name);
            if (oldValue != value) 
            {
        	    setProperty(name, value.ToString());
            }
        }

        public void setValue(String name, int value) 
        {
            int oldValue = getInt(name);
            if (oldValue != value) 
            {
        	    setProperty(name, value.ToString());
            }
        }

        public void setValue(String name, byte value) 
        {
            int oldValue = getByte(name);
            if (oldValue != value) 
            {
        	    setProperty(name, value.ToString());
            }
        }
    
        public void setValue(String name, long value) 
        {
            long oldValue = getLong(name);
            if (oldValue != value) 
            {
        	    setProperty(name, value.ToString());
            }
        }

        public void setValue(String name, String value) 
        {
            String oldValue = getString(name);
            if (!oldValue.Equals(value)) 
            {
                if (value != null) 
                {
            	    setProperty(name, value);
                }
            }
        }

        public void setPassword(String name, String value) 
        {
    	    String oldValue = getPassword(name);
    	    if (!oldValue.Equals(value)) 
            {
    		    if (value != null) 
                {
    			    try 
                    {
                	    setProperty(name, encrypt(value, KEY));
    			    } 
                    catch { }
    		    }
    	    }
        }
    
        public void setValue(String name, bool value) {
            bool oldValue = getBoolean(name);
            if (oldValue != value) 
            {
        	    setProperty(name, value == true ? Config.TRUE : Config.FALSE);
            }
        }

        public void setValue(String name, DateTime value) 
        {
            DateTime oldValue = getDate(name);
            if (!oldValue.Equals(value)) {
        	    setProperty(name, value.ToString());
            }
        }

        public void save(String filename) 
        {
            try 
            {
                String[] lines = new String[properties.Count];
                int idx = 0;
                foreach (String key in properties.Keys)
                {
                    lines[idx] = key + "=" + getProperty(key);
                    idx++;
                }
                System.IO.File.WriteAllLines(filename, lines);
            } 
            catch { }
        }
    
        public void load(String filename) 
        {
    	    try 
            {
                properties = new Dictionary<string,string>();
                String[] lines = System.IO.File.ReadAllLines(filename);
                for (int idx = 0; idx < lines.Length; idx++) 
                {
                    int pos = lines[idx].IndexOf("=");
                    if (pos >= 0)
                    {
                        String key = lines[idx].Substring(0, pos);
                        String value = lines[idx].Substring(pos + 1);
                        setProperty(key, value);
                    }
                }
    	    } 
            catch { }
        }
    
	    public static String encrypt(String value, String key) 
        {
            byte[] bytes = ASCIIEncoding.ASCII.GetBytes(key);
            DESCryptoServiceProvider provider = new DESCryptoServiceProvider();
            MemoryStream memory = new MemoryStream();
            CryptoStream crypto = new CryptoStream(memory, provider.CreateEncryptor(bytes, bytes), CryptoStreamMode.Write);
            StreamWriter writer = new StreamWriter(crypto);
            writer.Write(value);
            writer.Flush();
            crypto.FlushFinalBlock();
            writer.Flush();
            return Convert.ToBase64String(memory.GetBuffer(), 0, (int)memory.Length);
	    }
	
	    public static String decrypt(String value, String key) 
        {
            byte[] bytes = ASCIIEncoding.ASCII.GetBytes(key);
            DESCryptoServiceProvider provider = new DESCryptoServiceProvider();
            MemoryStream memory = new MemoryStream(Convert.FromBase64String(value));
            CryptoStream crypto = new CryptoStream(memory, provider.CreateDecryptor(bytes, bytes), CryptoStreamMode.Read);
            StreamReader reader = new StreamReader(crypto);
            return reader.ReadToEnd();
	    }

    }
}
