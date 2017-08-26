//  Base64Converter.cs
//  http://kan-kikuchi.hatenablog.com/entry/Numeration
//
//  Created by kan.kikuchi on 2016.11.22.

//using UnityEditor;
using System;
using System.Collections.Generic;

/// <summary>
/// 32進数の相互変換を行うクラス
/// </summary>
public class Base32Utility
{

    //32進数で使う文字
    private readonly static List<char> CHAR_LIST = new List<char>(){
    '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 
    'a', 'c', 'd', 'e', 'f', 'g', 'h', 'j', 'k', 'm', 
    'n', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 
    'y', 'z'
  };

    //=================================================================================
    //32進数に変換
    //=================================================================================

    /// <summary>
    /// 2進数を32進数に変換する
    /// </summary>
    public static string BinaryToBase32(string binaryNum)
    {

        //5桁(32 = 2^5)毎に区切れるように、足りない分だけ0を追加する
        int digit = (1 + ((int)(binaryNum.Length - 1) / 5)) * 5;
        binaryNum = binaryNum.PadLeft(digit, '0');

        //5桁毎に変換し、入力値を32進数で表した文字列を作成
        string base32 = "";

        for (int i = 0; i < binaryNum.Length; i += 5)
        {
            int no = Convert.ToInt32(binaryNum.Substring(i, 5), 2);
            base32 += CHAR_LIST[no];
        }

        return base32;
    }

    /// <summary>
    /// 8進数を32進数に変換する
    /// </summary>
    public static string OctalToBase32(string octalNum)
    {
        //8進数→10進数→2進数→32進数
        return BinaryToBase32(Convert.ToString(Convert.ToInt64(octalNum, 8), 2));
    }

    /// <summary>
    /// 10進数を32進数に変換する
    /// </summary>
    public static string DecimalToBase32(long decimalNum)
    {
        //10進数→2進数→32進数
        return BinaryToBase32(Convert.ToString(decimalNum, 2));
    }

    /// <summary>
    /// 16進数を32進数に変換する
    /// </summary>
    public static string HexadecimalToBase32(string hexadecimalNum)
    {
        //16進数→10進数→2進数→32進数
        return BinaryToBase32(Convert.ToString(Convert.ToInt64(hexadecimalNum, 16), 2));
    }

    //=================================================================================
    //32進数を変換
    //=================================================================================

    /// <summary>
    /// 32進数を2進数に変換する
    /// </summary>
    public static string Base32ToBinary(string base32)
    {
        //一文字ずつ、5桁(32 = 2^5)の2進数に直していく
        string binaryNum = "";

        for (int i = 0; i < base32.Length; i++)
        {
            for (int listNo = 0; listNo < CHAR_LIST.Count; listNo++)
            {
                if (base32[i] == CHAR_LIST[listNo])
                {
                    binaryNum += Convert.ToString(listNo, 2).PadLeft(5, '0');
                    break;
                }
            }
        }

        return binaryNum;
    }

    /// <summary>
    /// 32進数を8進数に変換する
    /// </summary>
    public static string Base32ToOctal(string base32)
    {
        //32進数→2進数→10進数→8進数
        return Convert.ToString(Convert.ToInt64(Base32ToBinary(base32), 2), 8);
    }

    /// <summary>
    /// 32進数を10進数に変換する
    /// </summary>
    public static long Base64ToDecimal(string base32)
    {
        //64進数→2進数→10進数
        return Convert.ToInt64(Base32ToBinary(base32), 2);
    }

    /// <summary>
    /// 32進数を16進数に変換する
    /// </summary>
    public static string Base32ToHexadecimal(string base32)
    {
        //64進数→2進数→10進数→16進数
        return Convert.ToString(Convert.ToInt64(Base32ToBinary(base32), 2), 16);
    }

}