using System;

namespace WindowsApplication3
{
	/// <summary>
	/// 
	/// </summary>
	public class csector_node
	{
		public double [] relations  ;
		public  csector_node()
		{
			relations=new double [8] ;
			for (int i=0;i<8;i++)
			{
				relations[i]=new double() ;
				relations[i]=0 ;
			}
		}
	}
}
