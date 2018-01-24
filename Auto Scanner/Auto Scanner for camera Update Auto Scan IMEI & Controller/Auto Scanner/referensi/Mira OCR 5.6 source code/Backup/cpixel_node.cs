using System;
using System.Drawing ;
using System.IO ;

namespace WindowsApplication3
{
	/// <summary>
	/// 
	/// </summary>
	public class cpixel_node
	{
		
		public int x ;
		public int y;
		public int sector ;
		public int track ;
		public int relation ;
		public  cpixel_node()
		{
			x=1;
			y=1;
			sector=1;
			track=1;
			relation=1 ;
		}
		
		public void get_sector(int Xc,int Yc)
		{
			double theeta=0;
			float val=0 ;
			if (x-Xc>0 && y-Yc>0)
			{
				val=(y-Yc)/(x-Xc);
				theeta=Math.Atan(val);
				theeta=(Math.Abs(theeta)*180)/Math.PI ;
				sector=(int)theeta/90 ;
				return ;
			}
			else
			{	
				if (x-Xc<0 && y-Yc>0)
				{
					val=(y-Yc)/(x-Xc);
					theeta=Math.Atan(val);
					theeta=(theeta*180)/Math.PI ;
					theeta=180-Math.Abs(theeta) ;
					sector=(int)theeta/90 ;
					return ;
				}
				else
				{
					if (x-Xc<0 && y-Yc<0)
					{
						val=(y-Yc)/(x-Xc);
						theeta=Math.Atan(val);
						theeta=(theeta*180)/Math.PI ;
						theeta=Math.Abs(theeta)+180 ;
						sector=(int)theeta/90 ;
						return ;
					}
					else
					{
						if (x-Xc>0 && y-Yc<0)
						{
							val=(y-Yc)/(x-Xc);
							theeta=Math.Atan(val);
							theeta=(theeta*180)/Math.PI ;
							theeta=360-Math.Abs(theeta) ;
							int sector1=(int)theeta/90 ;
							if (sector1>3)
								sector=0;
                            else
								sector=sector1 ;

							return ;
						}		

						else
						{
							if (x-Xc==0)
							{
								if (y-Yc>0)
								{
									sector=1 ;
									return ;
								}
								else
								{
									sector=3 ;
									return ;
								}
							}
							if (y-Yc==0)
							{
								if(x-Xc>0)
									sector=0 ;

								else
									sector=2 ;

							}

						}
					}
				}
			
			}
		}
	public	void get_track(int Xc,int Yc,double track_step)
		{
			double ri=0;
			float xtot=0 ;
			float ytot=0 ;
		double temp=0 ;
			xtot=Xc-x ;
			xtot*=xtot ; // square xtot
			ytot=Yc-y ;
			ytot*=ytot ;

			ri=xtot+ytot ;
			ri=Math.Sqrt(ri) ;
			temp=ri/track_step ;
			//if (temp==5)
			//	track=4 ;
			//else
				track=(int) temp ;
		


		}

	public	void get_relation(Bitmap pict)
		{
			if(x-1>=pict.Width && y-1>=pict.Height)
			{
				if(pict.GetPixel(x-1,y-1)==Color.FromArgb(0,0,0))
				{
					relation=0 ;
					return ;
				}
			}

			if(y-1>=pict.Height)
			{
				if(pict.GetPixel(x,y-1)==Color.FromArgb(0,0,0))
				{
					relation=1;
					return ;
				}
			}
			if(x+1<=pict.Width && y-1>=pict.Height)
			{
				if(pict.GetPixel(x+1,y-1)==Color.FromArgb(0,0,0))
				{
					relation=2 ;
					return ;
				}
			}
			
			if(x-1>=pict.Width)
			{
				if(pict.GetPixel(x-1,y)==Color.FromArgb(0,0,0))
				{
					relation=3 ;
					return ;
				}
			}
			if(x+1<=pict.Width)
			{
				if(pict.GetPixel(x+1,y)==Color.FromArgb(0,0,0))
				{
					relation=4 ;
					return ;
				}
			}
			if(x-1>=pict.Width&&y+1<=pict.Height)
			{
				if(pict.GetPixel(x-1,y+1)==Color.FromArgb(0,0,0))
				{
					relation=5 ;
					return ;
				}
			}
			if(x-1>=pict.Width&&y-1>=pict.Height)
			{
				if(pict.GetPixel(x,y+1)==Color.FromArgb(0,0,0))
				{
					relation=6 ;
					return ;
				}
			}
			if(x+1<=pict.Width && y+1<=pict.Height)
			{
				if(pict.GetPixel(x+1,y+1)==Color.FromArgb(0,0,0))
				{
					relation=7 ;
					return ;
				}
			}
		}
		public void segmax(int xm)
		{
			x=xm;
		}
		public void segmay(int ym)
		{
			y=ym ;
		}

	
	}
}
