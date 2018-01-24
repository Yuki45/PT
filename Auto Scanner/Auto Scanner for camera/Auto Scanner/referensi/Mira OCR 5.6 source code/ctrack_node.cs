using System;

namespace WindowsApplication3
{
	public class ctrack_node
	{
		public csector_node [] sectors  ;
		public  ctrack_node()
		{
			sectors=new csector_node [4] ;
			for(int i=0;i<4;i++)
			{
				sectors[i]=new csector_node() ;
			}
		}
	}
}
