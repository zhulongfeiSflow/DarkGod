/****************************************************
	文件：PETools.cs
	作者：朱龙飞
	邮箱: 398670608@qq.com
	日期：2019/07/07 11:02   	
	功能：工具类
*****************************************************/


public class PETools
{
    public static int RDint(int min, int max, System.Random rd = null)
    {
        if (rd == null)
        {
            rd = new System.Random();
        }
        int val = rd.Next(min, max + 1);
        return val;
    }
}
