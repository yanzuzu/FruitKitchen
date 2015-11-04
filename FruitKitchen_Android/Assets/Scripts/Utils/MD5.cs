using System.Collections;
using System.Collections.Generic;
using System.Text;

public class MD5 {
	
	static public string ComputeUTF8( string buffer ){
		return new MD5().Begin().InputDataUTF8(buffer).End().Value;
	}
	
	static public string Compute( string buffer ){
		return new MD5().Begin().InputData(buffer).End().Value;
	}
	static public string Compute( byte[] buffer ){
		return new MD5().Begin().InputData(buffer).End().Value;
	}

	private System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create();
	private string hash_ = "";
	public string Value{ get{ return hash_;} }
	public MD5(){}
	private MD5 Begin(){ return this;}
	private MD5 End(){ return this;}
	private void makeHashString( byte[] bytes ) {
 		System.Text.StringBuilder sBuilder = new System.Text.StringBuilder();
        for (int i = 0; i < bytes.Length; ++i) {
			string ch = bytes[ i ].ToString("x2");
            sBuilder.Append( ch );
        }
        hash_ = sBuilder.ToString();
	}
	
	private MD5 InputData( string values ) {
		byte[] ary = System.Text.Encoding.Unicode.GetBytes( values );
		makeHashString( md5.ComputeHash( ary ) );
		return this;
	}
	
	private MD5 InputDataUTF8( string values ) {
		byte[] ary = System.Text.Encoding.UTF8.GetBytes( values );
		makeHashString( md5.ComputeHash( ary ) );
		return this;
	}
	
	private MD5 InputData( byte[] values ) {
		makeHashString( md5.ComputeHash( values ) );
		return this;
	}
}