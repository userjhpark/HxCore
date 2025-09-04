using System;
using System.Collections.Generic;
using System.Text;

namespace HxCore
{
    /*
     * 가 to MD5
    CryptAPI : d1457b72c3fb323a2671125aef3eab5d //ASCII
    Oracle : 795d6a1c51f0ac4b0856f5cac1846f3f //Default
    PHP : c0c6e70680bf5a0abf36c4a5b1f3a1b2 //UTF8
    NET : c0c6e70680bf5a0abf36c4a5b1f3a1b2 //UTF8

    System.Security.Cryptography.MD5 Hash Encoding Type별 결과
    Default : 795d6a1c51f0ac4b0856f5cac1846f3f
    ASCII : d1457b72c3fb323a2671125aef3eab5d
    UTF7 : 779ce439200f73f6ca6202eab4149b2f
    UTF8 : c0c6e70680bf5a0abf36c4a5b1f3a1b2
    UTF32 : 63827f7be00d03198fb4b37176d511ec
    Unicode : 20ce2f5ba7df349d18fa3931cda4c477
    BigEndianUnicode : d6057b38477fd0b459e964e7cc49c626
     * */
    public enum HxEncodingType
    {
        None = 0,
        Default,
        ASCII,
        UTF7,
        UTF8,
        UTF32,
        Unicode,
        BigEndianUnicode,
        //CryptApiMD5AsciiEncoding = Ascii,
        //PhpMD5DefaultEncoding = UTF8, //EUC-KR, CP949, UTF-8, ISO-8859-1
        //OracleRawToHexMD5DefaultEncoding = Default, //KO16KSC5601, KO16MSWIN949, UTF8, AL32UTF8, US7ASCII, WE8ISO8859P1
    }

    /*
     * 
<?php
$value = "가"; //UTF8
//$value = iconv("UTF-8", "ISO-8859-1", $value); // ASCII
$value = iconv("UTF-8", "CP949", $value); // Default //EUC-KR
echo md5($value);
?>


SQL>
select
--CONVERT(char, dest_char_set, source_char_set)
CONVERT('가', 'AL32UTF8', 'KO16MSWIN949'), --KO16KSC5601, KO16MSWIN949, UTF8, AL32UTF8,US7ASCII
LOWER(
    RAWTOHEX(
        UTL_RAW.CAST_TO_RAW(
            sys.dbms_obfuscation_toolkit.md5(input_string => '가')
        )
    )
) as MD5hash
,
LOWER(
    RAWTOHEX(
        UTL_RAW.CAST_TO_RAW(
            sys.dbms_obfuscation_toolkit.md5(input_string => CONVERT('가', 'AL32UTF8', 'KO16MSWIN949'))
        )
    )
)
, UTL_RAW.CAST_TO_RAW('가')
--, UTL_RAW.CAST_TO_nvarchar2('가')
from dual;
/
     * */

    //public enum HxValueExistType
    //{
    //    None,
    //    First,
    //    Last,
    //    Error
    //}


    public enum HxRegexPatternType
    {
        Numberic,
        WebUri
    }
}



