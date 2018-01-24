/******************************************************************************
* File name : MMCWHP.h
* Version   : 1.54
* Date      : 2011.02.16
* Version history
*     2010.07.23 : 1.5  : Brand Transition to RS Automation RS OEMAX
*     2010.08.13 : 1.51 : Added get_option_io_bit
*     2011.01.26 : 1.53 : Boundary check enabled
*     2011.02.16 : 1.54 : [Bug Fix] Control time setting
******************************************************************************/

#ifdef __cplusplus
extern "C" {
#endif

/*---------------------------------------------------------------------------*/
/* True and False                                                            */
/*---------------------------------------------------------------------------*/
#define		TRUE				1
#define		FALSE				0

/*---------------------------------------------------------------------------*/
/* High and Low                                                              */
/*---------------------------------------------------------------------------*/
#define		HIGH				1
#define		LOW  				0

/*---------------------------------------------------------------------------*/
/* Circle Move Direction                                                     */
/*---------------------------------------------------------------------------*/
#define		CIR_CCW 			1
#define		CIR_CW  			0

/*---------------------------------------------------------------------------*/
/* Coordinate  Direction                                                     */
/*---------------------------------------------------------------------------*/
#define		CORD_CCW 			1
#define		CORD_CW  			0

/*---------------------------------------------------------------------------*/
/* Encoder  Direction                                                        */
/*---------------------------------------------------------------------------*/
#define		ENCODER_CCW 		1
#define		ENCODER_CW  		0

/*---------------------------------------------------------------------------*/
/* Event Source Status defines                                               */
/*---------------------------------------------------------------------------*/
#define		ST_NONE					0x0000
#define		ST_HOME_SWITCH			0x0001
#define		ST_POS_LIMIT			0x0002
#define		ST_NEG_LIMIT   			0x0004
#define		ST_AMP_FAULT			0x0008
#define		ST_A_LIMIT    			0x0010
#define		ST_V_LIMIT  			0x0020
#define		ST_X_NEG_LIMIT 			0x0040
#define		ST_X_POS_LIMIT			0x0080
#define		ST_ERROR_LIMIT			0x0100
#define		ST_PC_COMMAND  			0x0200
#define		ST_OUT_OF_FRAMES    	0x0400
#define		ST_AMP_POWER_ONOFF  	0x0800
#define		ST_ABS_COMM_ERROR   	0x1000
#define		ST_INPOSITION_STATUS	0x2000
#define		ST_RUN_STOP_COMMAND 	0x4000 		
#define		ST_COLLISION_STATE  	0x8000 		
#define		ST_PAUSTATE_STATE  		0x10000 		

/*---------------------------------------------------------------------------*/
/* Event defines                                                             */
/*---------------------------------------------------------------------------*/
#define		NO_EVENT			0 	/* ignore a condition */
#define		STOP_EVENT			1	/* generate a stop event */
#define 	E_STOP_EVENT		2 	/* generate an e_stop event */
#define		ABORT_EVENT			3 	/* disable PID control, and disable the amplifier */

/*---------------------------------------------------------------------------*/
/* Digital Filter Defines                                                    */
/*---------------------------------------------------------------------------*/
#define		GAIN_NUMBER			5	/* elements expected get/set_filter(...) */
#define		GA_P				0	/* proportional gain */
#define		GA_I				1	/* integral gain */
#define		GA_D				2	/* derivative gain-damping term */
#define		GA_F       			3	/* velocity feed forward */
#define		GA_ILIMIT 			4	/* integration summing limit */

/*---------------------------------------------------------------------------*/
/* Error Defines                                                             */
/*---------------------------------------------------------------------------*/
#define		MAX_ERROR_LEN							80	/* maximum length for error massage string */
#define		MMC_OK									0	/* no problems */
#define		MMC_NOT_INITIALIZED						1	/* be sure to call mmc_init(...) */
#define		MMC_TIMEOUT_ERR							2	/* DPRAM Communication Error */
#define		MMC_INVALID_AXIS						3	/* axis out of range or not configured */
#define		MMC_ILLEGAL_ANALOG						4	/* analog channel < 0 or > 4 */
#define		MMC_ILLEGAL_IO							5	/* illegal I/O port */
#define		MMC_ILLEGAL_PARAMETER					6	/* move with zero accel or velocity */
#define		MMC_NO_MAP								7 	/* The map_axes(...) funcation has not been called */
#define		MMC_AMP_FAULT							8 	/* amp fault occured */
#define		MMC_ON_MOTION							9 	/* Motion is not completed */
#define		MMC_NON_EXIST							10	/* MMC Board is not exist */
#define		MMC_BOOT_OPEN_ERROR						11	/* MMC Boot File Read/Write Error*/
#define		MMC_CHKSUM_OPEN_ERROR					12	/* MMC Checksum File Read/Write Error*/
#define		MMC_WINNT_DRIVER_OPEN_ERROR				13	/* MMC Windows NT Driver Open Error*/
#define		MMC_EVENT_OCCUR_ERROR					14
#define		MMC_AMP_POWER_OFF     					15
#define		MMC_DATA_DIRECTORY_OPEN_ERROR			16
#define		MMC_INVALID_CPMOTION_GROUP				17
#define		MMC_VELOCITY_ILLEGAL_PARAMETER			18	/* move with zero accel or velocity */
#define		MMC_ACCEL_ILLEGAL_PARAMETER				19	/* move with zero accel or velocity */
#define		FUNC_ERR								-1	/* Function Error				*/

//homing api return
#define		MMC_HOMING_DONE							0
#define		MMC_HOMING_ERROR						1
#define		MMC_HOMING_WORKING						2
#define		MMC_HOMING_TIMEOUT						3
#define		MMC_HOMING_STOP							4

/*---------------------------------------------------------------------------*/
/* General Defines                                                           */
/*---------------------------------------------------------------------------*/
#define		POSITIVE		1
#define		NEGATIVE		0

/*---------------------------------------------------------------------------*/
/* Motor Type                                                                */
/*---------------------------------------------------------------------------*/
#define		SERVO_MOTOR		0
#define		STEPPER			1
#define		MICRO_STEPPER	2

/*---------------------------------------------------------------------------*/
/* Feedback Configuration                                                    */
/*---------------------------------------------------------------------------*/
#define		FB_ENCODER		0
#define		FB_UNIPOLAR		1
#define		FB_BIPOLAR		2

/*---------------------------------------------------------------------------*/
/* Control_Loop Method                                                       */
/*---------------------------------------------------------------------------*/
#define		OPEN_LOOP		0
#define		CLOSED_LOOP		1
#define		SEMI_LOOP		2

/*---------------------------------------------------------------------------*/
/* Control Method                                                            */
/*---------------------------------------------------------------------------*/
#define		V_CONTROL		0
#define		T_CONTROL		1

#define		IN_STANDING		0
#define		IN_ALWAYS		1

#define		TWO_PULSE		0
#define		SIGN_PULSE		1
/*---------------------------------------------------------------------------*/
/* Limit Vlaue                                                               */
/*---------------------------------------------------------------------------*/
#define		MMC_ACCEL_LIMIT		25000
#define		MMC_VEL_LIMIT		5000000
#define		MMC_POS_SW_LIMIT	2147483640
#define		MMC_NEG_SW_LIMIT	-2147483640
#define		MMC_ERROR_LIMIT		35000
#define		MMC_PULSE_RATIO		255

/*---------------------------------------------------------------------------*/
// Homing Config and Option
/*---------------------------------------------------------------------------*/
//homing direction
#define		MMC_HOMING_DIR_MAIN_NEG       0x0000      // main homing direction negative
#define		MMC_HOMING_DIR_MAIN_POS       0x0001      // main homing direction positive
#define		MMC_HOMING_DIR_MARKER_NEG     0x0000      // marker homing direction negative
#define		MMC_HOMING_DIR_MARKER_POS     0x0002      // marker homing direction positive
//homing config
#define		MMC_HOMING_EDGE_HOME_NEG      0x0001      // homing edge of home sensor negative
#define		MMC_HOMING_EDGE_HOME_POS      0x0002      // homing edge of home sensor positive
#define		MMC_HOMING_EDGE_LN			  0x0004      // homing edge of negative limit
#define		MMC_HOMING_EDGE_LP			  0x0008      // homing edge of positive limit
#define		MMC_HOMING_MARKER_USE	      0x0010	  // homing use marker
		
/*---------------------------------------------------------------------------*/
/* Type Define                                                               */
/*---------------------------------------------------------------------------*/
#define		CHAR		char
#define		INT			short
#define		LONG		long
#define		ULONG		unsigned long
#define		FLOAT		float
#define		DOUBLE		double
	
#define		pCHAR		CHAR *
#define     pINT		INT *
#define		pLONG		LONG *
#define		pULONG		unsigned long *
#define		pFLOAT		FLOAT *
#define		pDOUBLE		double *

#define		VOID		void

#ifdef WIN32
/* WIN32 programs which do not use the WINAPI doesn't need to include the    */
/* "windows.h", so the WIN32 sections are moved here.                        */
#define		API			_stdcall
#else
/* Static library can use any reference according to it's compiling model.   */
/* In this code, but, large model assumed and not tested building any others.*/
#define 	API
#endif

#ifdef	WIN32
/* In windows programs, there is no easy way to export its global variable,  */
/* other than functions to other module referencing this DLL.                */
INT API get_mmc_error( VOID);
INT	API get_version( VOID);
INT	API get_axis_num( VOID);
INT	API get_bd_num( VOID);
INT	API get_velocity(INT ax);
#else
extern INT mmc_error;
#endif

INT API mmc_initx(INT len, pLONG dp_addr);
INT	API version_chk(INT bn, pINT ver);
INT	API motion_fpga_version_chk(INT bn, pINT ver);
INT API option_fpga_version_chk(INT bn, pINT ver);
INT API set_dpram_addr(INT bdnum, LONG addr);
INT API get_dpram_addr(INT bdnum, pLONG addr);
INT API error_message(INT code, pCHAR dst);
pCHAR API _error_message(INT code);
INT API save_boot_frame(VOID);

INT API mmc_axes(INT, pINT);
INT API mmc_all_axes(VOID);
INT API get_stepper(INT ax);
INT API set_stepper(INT ax);
INT API set_servo(INT ax);
INT API set_feedback(INT ax, INT device);
INT API get_feedback(INT ax, pINT device);
INT API set_closed_loop(INT ax, INT loop);
INT API get_closed_loop(INT ax, pINT loop);
INT API set_unipolar(INT ax, INT state);
INT API get_unipolar(INT ax);

INT API set_abs_encoder(INT ax);
INT API get_micro_stepper(INT ax);
INT API set_micro_stepper(INT ax);

INT API mmcDelay(LONG duration);
INT API mmc_dwell(INT ax, LONG duration);
INT API mmc_io_trigger(INT ax, INT bitNo, INT state);

INT API get_counter(INT ax, pDOUBLE pos);
INT API get_sync_position(pDOUBLE pos_m, pDOUBLE pos_s);

INT API set_position(INT ax, DOUBLE pos);
INT API get_position(INT ax, pDOUBLE pos);
INT API set_command(INT ax, DOUBLE pos);
INT API get_command(INT ax, pDOUBLE pos);
INT API get_error(INT ax, pDOUBLE error);
INT API get_com_velocity(INT ax);
INT API get_act_velocity(INT ax);

INT API in_sequence(INT ax);
INT API in_motion(INT ax);
INT API in_position(INT ax);
INT API frames_left(INT ax);
INT API motion_done(INT ax);
INT API axis_done(INT ax);
INT API axis_state(INT ax);
INT API axis_source(INT ax);
LONG API axis_sourcex(INT ax);
INT API clear_status(INT ax);
INT API frames_clear(INT ax);

INT API map_axes(INT n_axes, pINT map_array);
INT API set_move_speed(DOUBLE speed);
INT API set_move_accel(INT accel);
INT API set_arc_division(DOUBLE degrees);
INT API all_done(VOID);
INT API move_2(DOUBLE x, DOUBLE y);
INT API move_3(DOUBLE x, DOUBLE y, DOUBLE z);
INT API move_4(DOUBLE x, DOUBLE y, DOUBLE z, DOUBLE w);
INT API move_n(pDOUBLE x);
INT API move_2ax(INT ax1, INT ax2, DOUBLE x, DOUBLE y, DOUBLE vel, INT acc);
INT API move_3ax(INT ax1, INT ax2, INT ax3, DOUBLE x, DOUBLE y, DOUBLE z, DOUBLE vel, INT acc);
INT API move_4ax(INT ax1, INT ax2, INT ax3, INT ax4, DOUBLE x, DOUBLE y, DOUBLE z, DOUBLE w, DOUBLE vel, INT acc);
INT API move_nax(INT len, pINT ax, pDOUBLE pos, DOUBLE vel, INT acc);
INT API move_2axgr(INT gr, INT ax1, INT ax2, DOUBLE x, DOUBLE y, DOUBLE vel, INT acc);
INT API move_3axgr(INT gr, INT ax1, INT ax2, INT ax3, DOUBLE x, DOUBLE y, DOUBLE z, DOUBLE vel, INT acc);
INT API move_4axgr(INT gr, INT ax1, INT ax2, INT ax3, INT ax4, DOUBLE x, DOUBLE y, DOUBLE z, DOUBLE w, DOUBLE vel, INT acc);

INT API smove_2(DOUBLE x, DOUBLE y);
INT API smove_3(DOUBLE x, DOUBLE y, DOUBLE z);
INT API smove_4(DOUBLE x, DOUBLE y, DOUBLE z, DOUBLE w);
INT API smove_n(pDOUBLE x);
INT API smove_2ax(INT ax1, INT ax2, DOUBLE x, DOUBLE y, DOUBLE vel, INT acc);
INT API smove_3ax(INT ax1, INT ax2, INT ax3, DOUBLE x, DOUBLE y, DOUBLE z, DOUBLE vel, INT acc);
INT API smove_4ax(INT ax1, INT ax2, INT ax3, INT ax4, DOUBLE x, DOUBLE y, DOUBLE z, DOUBLE w, DOUBLE vel, INT acc);
INT API smove_nax(INT len, pINT ax, pDOUBLE pos, DOUBLE vel, INT acc);
INT API smove_2axgr(INT gr, INT ax1, INT ax2, DOUBLE x, DOUBLE y, DOUBLE vel, INT acc);
INT API smove_3axgr(INT gr, INT ax1, INT ax2, INT ax3, DOUBLE x, DOUBLE y, DOUBLE z, DOUBLE vel, INT acc);
INT API smove_4axgr(INT gr, INT ax1, INT ax2, INT ax3, INT ax4, DOUBLE x, DOUBLE y, DOUBLE z, DOUBLE w, DOUBLE vel, INT acc);

INT API arc_2(DOUBLE x_center, DOUBLE y_center, DOUBLE angle);
INT API arc_2ax(INT ax1, INT ax2, DOUBLE x_center, DOUBLE y_center, DOUBLE angle, DOUBLE vel, INT acc);
INT API spl_line_move2(pDOUBLE pnt, DOUBLE vel, INT acc);
INT API spl_line_move3(pDOUBLE pnt, DOUBLE vel, INT acc);
INT API spl_line_move2ax(INT ax1, INT ax2, pDOUBLE pnt, DOUBLE vel, INT acc);
INT API spl_line_move3ax(INT ax1, INT ax2, INT ax3, pDOUBLE pnt, DOUBLE vel, INT acc);

INT API spl_arc_move2(DOUBLE x_center, DOUBLE y_center, pDOUBLE pnt, DOUBLE vel, INT acc, INT cdir);
INT API spl_arc_move3(DOUBLE x_center, DOUBLE y_center, pDOUBLE pnt, DOUBLE vel, INT acc, INT cdir);
INT API spl_arc_move2ax(INT ax1, INT ax2, DOUBLE x_center, DOUBLE y_center, pDOUBLE pnt, DOUBLE vel, INT acc, INT cdir);
INT API spl_arc_move3ax(INT ax1, INT ax2, INT ax3, DOUBLE x_center, DOUBLE y_center, pDOUBLE pnt, DOUBLE vel, INT acc, INT cdir);

INT API spl_move(INT len, INT ax1, INT ax2, INT ax3, pDOUBLE pnt1, pDOUBLE pnt2, pDOUBLE pnt3, DOUBLE vel, INT acc);
INT API rect_move(INT ax1, INT ax2, pDOUBLE pnt, DOUBLE vel, INT acc);
INT API set_spl_auto_off(INT bd_num,INT mode);

INT API start_move(INT ax, DOUBLE pos, DOUBLE vel, INT acc);
INT API move(INT ax, DOUBLE pos,DOUBLE vel, INT acc);
INT API start_r_move(INT ax, DOUBLE pos, DOUBLE vel, INT acc);
INT API r_move(INT ax, DOUBLE pos,DOUBLE vel, INT acc);
INT API start_s_move(INT ax, DOUBLE pos, DOUBLE vel, INT acc);
INT API s_move(INT ax, DOUBLE pos,DOUBLE vel, INT acc);
INT API start_rs_move(INT ax, DOUBLE pos, DOUBLE vel, INT acc);
INT API rs_move(INT ax, DOUBLE pos,DOUBLE vel, INT acc);
INT API start_t_move(INT ax, DOUBLE pos, DOUBLE vel, INT acc, INT dcc);
INT API t_move(INT ax, DOUBLE pos,DOUBLE vel, INT acc, INT dcc);
INT API start_ts_move(INT ax, DOUBLE pos, DOUBLE vel, INT acc, INT dcc);
INT API ts_move(INT ax, DOUBLE pos,DOUBLE vel, INT acc, INT dcc);
INT API start_tr_move(INT ax, DOUBLE pos, DOUBLE vel, INT acc, INT dcc);
INT API tr_move(INT ax, DOUBLE pos,DOUBLE vel, INT acc, INT dcc);
INT API start_trs_move(INT ax, DOUBLE pos, DOUBLE vel, INT acc, INT dcc);
INT API trs_move(INT ax, DOUBLE pos,DOUBLE vel, INT acc, INT dcc);
INT API start_t_move_all(INT len, pINT ax, pDOUBLE pos, pDOUBLE vel, pINT acc,pINT dcc);
INT API t_move_all(INT len, pINT ax, pDOUBLE pos, pDOUBLE vel, pINT acc, pINT dcc);
INT API start_ts_move_all(INT len, pINT ax, pDOUBLE pos, pDOUBLE vel, pINT acc,pINT dcc);
INT API ts_move_all(INT len, pINT ax, pDOUBLE pos,pDOUBLE vel, pINT acc, pINT dcc);


INT API start_move_all(INT len, pINT ax, pDOUBLE pos, pDOUBLE vel, pINT acc);
INT API move_all(INT len, pINT ax, pDOUBLE pos,pDOUBLE vel, pINT acc);
INT API start_s_move_all(INT len, pINT ax, pDOUBLE pos, pDOUBLE vel, pINT acc);
INT API s_move_all(INT len, pINT ax, pDOUBLE pos,pDOUBLE vel, pINT acc);
INT API wait_for_done( INT ax);
INT API wait_for_all(INT len, pINT ax);
INT API v_move(INT ax, DOUBLE vel, INT acc);

INT API set_positive_sw_limit(INT ax, DOUBLE limit, INT action);
INT API get_positive_sw_limit(INT ax, pDOUBLE limit, pINT action);
INT API set_negative_sw_limit(INT ax, DOUBLE limit, INT action);
INT API get_negative_sw_limit(INT ax, pDOUBLE limit, pINT action);

INT API get_accel_limit(INT ax, pINT limit);
INT API set_accel_limit(INT ax, INT limit);
INT API fset_accel_limit(INT ax, INT limit);
INT API get_vel_limit(INT ax, pDOUBLE limit);
INT API set_vel_limit(INT ax, DOUBLE limit);

INT API set_positive_limit(INT ax, INT act);
INT API get_positive_limit(INT ax, pINT act);
INT API set_negative_limit(INT ax, INT act);
INT API get_negative_limit(INT ax, pINT act);
INT API set_in_position(INT ax, DOUBLE pos);
INT API get_in_position(INT ax, pDOUBLE pos);
INT API set_error_limit(INT ax, DOUBLE limit, INT action);
INT API get_error_limit(INT ax, pDOUBLE limit, pINT action);
INT API set_positive_level(INT ax, INT level);
INT API get_positive_level(INT ax, pINT level);
INT API set_negative_level(INT ax, INT level);
INT API get_negative_level(INT ax, pINT level);

INT API home_switch(INT ax);
INT API pos_switch(INT ax);
INT API neg_switch(INT ax);
INT API amp_fault_switch(INT ax);
INT API set_io(INT port, LONG value);
INT API get_io(INT port, pLONG value);
INT API get_out_io(INT port, pLONG value);
INT API set_bit(INT bitNo);
INT API reset_bit(INT bitNo);

INT API set_option_io(INT port, LONG value);
INT API get_option_io(INT port, pLONG value);
INT API get_option_out_io(INT port, pLONG value);
INT API set_option_bit(INT bitNo);
INT API reset_option_bit(INT bitNo);
// 2010.08.13 SKKang
INT API get_option_io_bit(INT port, INT bit, pINT status);

INT API get_gain(INT ax, pLONG coeff);
INT API fget_gain(INT ax, pLONG coeff);
INT API set_gain(INT ax, pLONG coeff);
INT API fset_gain(INT ax, pLONG coeff);
INT API get_v_gain(INT ax, pLONG coeff);
INT API fget_v_gain(INT ax, pLONG coeff);
INT API set_v_gain(INT ax, pLONG coeff);
INT API fset_v_gain(INT ax, pLONG coeff);

INT API fset_p_integration(INT ax, INT mode);
INT API fset_v_integration(INT ax, INT mode);
INT API fget_p_integration(INT ax, pINT mode);
INT API fget_v_integration(INT ax, pINT mode);
INT API set_p_integration(INT ax, INT mode);
INT API get_p_integration(INT ax, pINT mode);
INT API set_v_integration(INT ax, INT mode);
INT API get_v_integration(INT ax, pINT mode);

INT API set_amp_enable(INT axno,INT state);
INT API get_amp_enable(INT axno,pINT state);
INT API amp_fault_reset(INT axno);
INT API amp_fault_set(INT axno);
INT API set_amp_enable_level(INT axno,INT level);
INT API get_amp_enable_level(INT axno,pINT level);
INT API fget_amp_enable_level(INT axno,pINT level);
INT API set_control(INT axno,INT control);
INT API get_control(INT axno, pINT control);
INT API set_electric_gear(INT axno, DOUBLE ratio);
INT API get_electric_gear(INT axno, pDOUBLE ratio);
INT API fget_electric_gear(INT axno, pDOUBLE ratio);
INT API set_step_mode(INT ax, INT mode);
INT API get_step_mode(INT ax, pINT mode);
INT API set_sync_map_axes(INT Master, INT Slave);
INT API get_sync_map_axes(pINT Master, pINT Slave);//2010.11.08 skkang
INT API set_sync_control(INT condition);
INT API get_sync_control(pINT condition);
INT API set_sync_gain(LONG syncgain);
INT API fset_sync_gain(LONG syncgain);
INT API get_sync_gain(pLONG syncgain);
INT API fget_sync_gain(pLONG syncgain);
INT API compensation_pos(INT len, pINT ax, pDOUBLE pos, pINT acc);

INT API set_pulse_ratio(INT axno, INT pgratio);
INT API fset_pulse_ratio(INT axno, INT pgratio);
INT API get_pulse_ratio(INT axno, pINT pgratio);

INT API set_stop(INT ax);
INT API set_stop_rate(INT ax, INT rate);
INT API get_stop_rate(INT ax, pINT rate);
INT API set_e_stop(INT ax);
INT API set_e_stop_rate(INT ax, INT rate);
INT API get_e_stop_rate(INT ax, pINT rate);

INT API set_home(INT ax, INT action);
INT API get_home(INT ax, pINT action);
INT API set_home_level(INT ax, INT level);
INT API get_home_level(INT ax, pINT level);
INT API set_index_required(INT ax, INT index);
INT API get_index_required(INT ax, pINT index);

INT API io_interrupt_enable(INT bn, INT state);
INT API io_interrupt_on_stop(INT ax, INT state);
INT API io_interrupt_on_e_stop(INT ax, INT state);
INT API io_interrupt_pcirq(INT bn, INT state);
INT API io_interrupt_pcirq_eoi(INT bn);

INT API set_amp_fault(INT ax, INT action);
INT API get_amp_fault(INT ax, pINT action);
INT API set_amp_fault_level(INT ax, INT level);
INT API get_amp_fault_level(INT ax, pINT level);
INT API fget_amp_fault_level(INT ax, pINT level);
INT API set_amp_reset_level(INT ax, INT level);
INT API get_amp_reset_level(INT ax, pINT level);
INT API fget_amp_reset_level(INT ax, pINT level);

INT API get_analog(INT channel, pINT value);
INT API set_dac_output(INT ax, INT voltage);
INT API get_dac_output(INT ax, pINT voltage);

INT API fset_stepper(INT ax);
INT API fset_servo(INT ax);
INT API fset_feedback(INT ax, INT device);
INT API fset_closed_loop(INT ax, INT loop);
INT API fset_unipolar(INT ax, INT state);

INT API fset_micro_stepper(INT ax);
INT API fget_micro_stepper(INT ax);

INT API fset_amp_fault(INT ax, INT action);

INT API fset_control(INT axno, INT control);
INT API fset_electric_gear(INT axno, DOUBLE ratio);
INT API fset_step_mode(INT ax, INT mode);

INT API fset_home(INT ax, INT action);
INT API fset_index_required(INT ax, INT index);

INT API fio_interrupt_enable(INT bn, INT state);
INT API fio_interrupt_on_stop(INT ax, INT state);
INT API fio_interrupt_on_e_stop(INT ax, INT state);
INT API fio_interrupt_pcirq(INT bn, INT state);

INT API fset_positive_sw_limit(INT ax, DOUBLE limit, INT action);
INT API fset_negative_sw_limit(INT ax, DOUBLE limit, INT action);
INT API fset_positive_limit(INT ax, INT action);
INT API fset_negative_limit(INT ax, INT action);
INT API fset_in_position(INT ax, DOUBLE pos);
INT API fset_error_limit(INT ax, DOUBLE limit, INT action);

INT API fset_stop_rate(INT ax, INT rate);
INT API fset_e_stop_rate(INT ax, INT rate);

INT API fget_stepper(INT ax);
INT API fget_feedback(INT ax, pINT device);
INT API fget_closed_loop(INT ax, pINT loop);
INT API fget_unipolar(INT ax);

INT API fget_amp_fault(INT ax, pINT action);

INT API fget_control(INT axno, pINT control);
INT API fget_step_mode(INT ax, pINT mode);

INT API fget_home(INT ax, pINT action);
INT API fget_index_required(INT ax, pINT index);

INT API fget_positive_sw_limit(INT ax, pDOUBLE limit, pINT action);
INT API fget_negative_sw_limit(INT ax, pDOUBLE limit, pINT action);
INT API fget_positive_limit(INT ax, pINT action);
INT API fget_negative_limit(INT ax, pINT action);
INT API fget_in_position(INT ax, pDOUBLE pos);
INT API fget_error_limit(INT ax, pDOUBLE limit, pINT action);

INT API fget_stop_rate(INT ax, pINT rate);
INT API fget_e_stop_rate(INT ax, pINT rate);

INT API set_interpolation(INT Len, pINT ax, pLONG idelt_s, INT flag);
INT API frames_interpolation(INT ax);

INT API v_move_stop(INT ax);
INT API set_inposition_level(INT ax, INT level);
INT API fset_inposition_level(INT ax, INT level);
INT API get_inposition_level(INT ax, pINT level);
INT API fget_inposition_level(INT ax, pINT level);

INT API controller_idle(INT ax);
INT API controller_run(INT ax);

INT API arm_latch(INT bn, INT state);
INT API latch_status(INT bn);
INT API get_latched_position(INT ax, pDOUBLE pos);
INT API latch(INT bn);

INT API set_timer(INT bn, LONG time);
INT API get_timer(INT bn, pLONG time);

INT API fget_home_level(INT ax, pINT level);
INT API fget_positive_level(INT ax, pINT level);
INT API fget_negative_level(INT ax, pINT level);

INT API fset_amp_fault_level(INT ax, INT level);
INT API fset_amp_reset_level(INT ax, INT level);
INT API fset_amp_enable_level(INT ax, INT level);
INT API fget_pulse_ratio(INT axno, pINT ratio);
INT API fset_home_level(INT ax, INT level);
INT API fget_accel_limit(INT ax, pINT limit);
INT API fget_vel_limit(INT ax, pDOUBLE limit);
INT API fset_vel_limit(INT ax, DOUBLE limit);
INT API fset_positive_level(INT ax, INT level);
INT API fset_negative_level(INT ax, INT level);

INT API set_io_mode(INT bd_num, INT mode);
INT API fset_io_mode(INT bd_num, INT mode);
INT API get_io_mode(INT bd_num, pINT mode);
INT API fget_io_mode(INT bd_num, pINT mode);
INT API get_io_num(INT ax, pINT val);

INT API set_analog_offset(INT ax, INT voltage);
INT API fset_analog_offset(INT ax, INT voltage);
INT API get_analog_offset(INT ax, pINT voltage);
INT API fget_analog_offset(INT ax, pINT voltage);

INT API set_inposition_required(INT ax, INT inposflag);
INT API fset_inposition_required(INT ax, INT inposflag);
INT API get_inposition_required(INT ax, pINT inposflag);
INT API fget_inposition_required(INT ax, pINT inposflag);

INT API set_coordinate_direction(INT ax, INT direc);
INT API fset_coordinate_direction(INT ax, INT direc);
INT API get_coordinate_direction(INT ax, pINT direc);
INT API fget_coordinate_direction(INT ax, pINT direc);

INT API set_encoder_direction(INT ax, INT direc);
INT API fset_encoder_direction(INT ax, INT direc);
INT API get_encoder_direction(INT ax, pINT direc);
INT API fget_encoder_direction(INT ax, pINT direc);

INT API set_axis_runstop(INT bd_num,INT mode);
INT API get_axis_runstop(INT bd_num,pINT mode);

INT API set_endless_rotationax(INT ax, INT status, INT resolution);
INT API fset_endless_rotationax(INT ax, INT status, INT resolution);
INT API get_endless_rotationax(INT ax, pINT status);
INT API fget_endless_rotationax(INT ax, pINT status);

INT API set_endless_linearax(INT ax, INT status, INT resolution);
INT API fset_endless_linearax(INT ax, INT status, INT resolution);
INT API get_endless_linearax(INT ax, pINT status);
INT API fget_endless_linearax(INT ax, pINT status);
INT API set_endless_range(INT ax, DOUBLE range);
INT API fset_endless_range(INT ax, DOUBLE range);
INT API get_endless_range(INT ax, pDOUBLE range);
INT API fget_endless_range(INT ax, pDOUBLE range);

INT API set_linear_all_stop_flag(INT bd_num, INT mode);
INT API get_linear_all_stop_flag(INT bd_num, pINT mode);

INT API get_command_rpm(INT ax, pINT rpm_val);
INT API get_encoder_rpm(INT ax, pINT rpm_val);

INT API set_amp_resolution(INT ax, INT resolution);
INT API fset_amp_resolution(INT ax, INT resolution);
INT API get_amp_resolution(INT ax, pINT resolution);
INT API fget_amp_resolution(INT ax, pINT resolution);

INT API set_collision_prevent_flag(INT bd_num, INT mode);
INT API get_collision_prevent_flag(INT bd_num, pINT mode);
INT API set_collision_prevent(INT max, INT sax, INT add_sub, INT non_equal, DOUBLE c_pos);

INT API set_abs_encoder_type(INT ax, INT type);
INT API get_abs_encoder_type(INT ax, pINT type);

INT API set_fast_read_encoder(INT ax, INT status);
INT API get_fast_read_encoder(INT ax, pINT status);

INT API set_control_timer(INT bn, INT time);
INT API fset_control_timer(INT bn, INT time);
INT API get_control_timer(INT bn, pINT time);
INT API fget_control_timer(INT bn, pINT time);
INT API spl_move_data(INT spl_num, INT len, INT ax1, INT ax2, INT ax3, pDOUBLE pnt1, pDOUBLE pnt2, pDOUBLE pnt3, DOUBLE vel, INT acc);
INT API spl_movex(INT spl_num, INT ax1, INT ax2, INT ax3);
INT API spl_arc_movenax(INT len, pINT ax, DOUBLE x_center, DOUBLE y_center, pDOUBLE pnt, DOUBLE vel, INT acc, INT cdir);
INT API spl_line_movenax(INT len, pINT ax, pDOUBLE pnt, DOUBLE vel, INT acc);

INT API set_mmc_led_num(INT bn);
INT API get_mmc_led_num(INT bn, pINT led_num);
INT API get_fast_position(INT ax, pDOUBLE pos);

INT API set_encoder_ratioa(INT ax, INT ratioa);
INT API fset_encoder_ratioa(INT ax, INT ratioa);
INT API get_encoder_ratioa(INT ax, pINT ratioa);
INT API fget_encoder_ratioa(INT ax, pINT ratioa);

INT API set_encoder_ratiob(INT ax, INT ratiob);
INT API fset_encoder_ratiob(INT ax, INT ratiob);
INT API get_encoder_ratiob(INT ax, pINT ratiob);
INT API fget_encoder_ratiob(INT ax, pINT ratiob);

INT API set_analog_limit(INT ax, LONG voltage);
INT API fset_analog_limit(INT ax, LONG voltage);
INT API get_analog_limit(INT ax, pLONG voltage);
INT API fget_analog_limit(INT ax, pLONG voltage);

INT API set_position_lowpass_filter(INT ax, DOUBLE hz);
INT API fset_position_lowpass_filter(INT ax, DOUBLE hz);
INT API get_position_lowpass_filter(INT ax, pDOUBLE hz);
INT API fget_position_lowpass_filter(INT ax, pDOUBLE hz);

INT API set_velocity_lowpass_filter(INT ax, DOUBLE hz);
INT API fset_velocity_lowpass_filter(INT ax, DOUBLE hz);
INT API get_velocity_lowpass_filter(INT ax, pDOUBLE hz);
INT API fget_velocity_lowpass_filter(INT ax, pDOUBLE hz);

INT API set_position_notch_filter(INT ax, DOUBLE hz);
INT API fset_position_notch_filter(INT ax, DOUBLE hz);
INT API get_position_notch_filter(INT ax, pDOUBLE hz);
INT API fget_position_notch_filter(INT ax, pDOUBLE hz);

INT API set_velocity_notch_filter(INT ax, DOUBLE hz);
INT API fset_velocity_notch_filter(INT ax, DOUBLE hz);
INT API get_velocity_notch_filter(INT ax, pDOUBLE hz);
INT API fget_velocity_notch_filter(INT ax, pDOUBLE hz);

INT API set_mmc_parameter_init(INT ax);
INT API axis_all_status(INT ax, pINT istatus, pLONG lstatus, pDOUBLE dstatus);
INT API axis_monitor_params(INT ax, pLONG lval, pINT ival );

INT API set_sensor_auto_off(INT ax, INT off);
INT API fset_sensor_auto_off(INT ax, INT off);
INT API get_sensor_auto_off(INT ax, pINT off);
INT API fget_sensor_auto_off(INT ax, pINT off);
INT API get_spline_move_num(INT bd_num, pINT num);

INT API set_servo_linear_flag(INT ax, INT l_flag);
INT API fset_servo_linear_flag(INT ax, INT l_flag);
INT API get_servo_linear_flag(INT ax, pINT l_flag);
INT API fget_servo_linear_flag(INT ax, pINT l_flag);

INT API spl_auto_line_move2ax(INT ax1, INT ax2, pDOUBLE pnt, DOUBLE vel, INT acc, INT auto_flag);
INT API spl_auto_line_move3ax(INT ax1, INT ax2, INT ax3, pDOUBLE pnt, DOUBLE vel, INT acc, INT auto_flag);
INT API spl_auto_arc_move2ax(INT ax1, INT ax2, DOUBLE x_center, DOUBLE y_center, pDOUBLE pnt, DOUBLE vel, INT acc, INT cdir, INT auto_flag);
INT API spl_auto_arc_move3ax(INT ax1, INT ax2, INT ax3, DOUBLE x_center, DOUBLE y_center, pDOUBLE pnt, DOUBLE vel, INT acc, INT cdir, INT auto_flag);
INT API spl_auto_arc_movenax(INT len, pINT ax, DOUBLE x_center, DOUBLE y_center, pDOUBLE pnt, DOUBLE vel, INT acc, INT cdir, INT auto_flag);
INT API spl_auto_line_movenax(INT len, pINT ax, pDOUBLE pnt, DOUBLE vel, INT acc, INT auto_flag);

INT API set_analog_direction(INT ax, INT dac_dir);
INT API fset_analog_direction(INT ax, INT dac_dir);
INT API get_analog_direction(INT ax, pINT dac_dir);
INT API fget_analog_direction(INT ax, pINT dac_dir);

INT API position_compare(INT index_sel, INT index_num, INT bitNo, INT ax1, INT ax2, INT latch, INT function, INT out_mode, DOUBLE pos, LONG time);
/* by rolstone -- Ver 1.2 : Reset position_compare function */ 
INT API position_compare_reset(INT bn);
INT API position_compare_enable(INT bn, INT flag);
INT API position_compare_index_clear(INT bn, INT index);
INT API position_compare_init(INT index_sel, INT ax1, INT ax2);
INT API position_compare_read(INT index_sel, INT ax, pDOUBLE pos);
INT API position_compare_bit(INT bdNum, INT bitNum, INT OnOff);

INT API spl_arc_deg_move2(DOUBLE x_center, DOUBLE y_center, pDOUBLE pnt, DOUBLE vel, INT acc, INT cdir);
INT API spl_arc_deg_move3(DOUBLE x_center, DOUBLE y_center, pDOUBLE pnt, DOUBLE vel, INT acc, INT cdir);

INT API spl_arc_deg_move2ax(INT ax1, INT ax2, DOUBLE x_center, DOUBLE y_center, pDOUBLE pnt, DOUBLE vel, INT acc, INT cdir);
INT API spl_arc_deg_move3ax(INT ax1, INT ax2, INT ax3, DOUBLE x_center, DOUBLE y_center, pDOUBLE pnt, DOUBLE vel, INT acc, INT cdir);
INT API spl_arc_deg_movenax(INT len, pINT ax, DOUBLE x_center, DOUBLE y_center, pDOUBLE pnt, DOUBLE vel, INT acc, INT cdir);

INT API spl_auto_arc_deg_move2ax(INT ax1, INT ax2, DOUBLE x_center, DOUBLE y_center, pDOUBLE pnt, DOUBLE vel, INT acc, INT cdir, INT auto_flag);
INT API spl_auto_arc_deg_move3ax(INT ax1, INT ax2, INT ax3, DOUBLE x_center, DOUBLE y_center, pDOUBLE pnt, DOUBLE vel, INT acc, INT cdir, INT auto_flag);
INT API spl_auto_arc_deg_movenax(INT len, pINT ax, DOUBLE x_center, DOUBLE y_center, pDOUBLE pnt, DOUBLE vel, INT acc, INT cdir, INT auto_flag);

INT API arc_3(DOUBLE x_center, DOUBLE y_center, DOUBLE angle, pDOUBLE pos);
INT API arc_3ax(INT ax1, INT ax2, INT ax3, DOUBLE x_center, DOUBLE y_center, DOUBLE angle, pDOUBLE pos, DOUBLE vel, INT acc);

INT API set_encoder_filter_num(INT ax, INT fn);
INT API fset_encoder_filter_num(INT ax, INT fn);
INT API get_encoder_filter_num(INT ax, pINT fn);
INT API fget_encoder_filter_num(INT ax, pINT fn);
INT	API	get_mmc_init_chkx();
INT	API	set_mmc_init_chkx(INT bn, INT val);

INT API spl_line_move1(pDOUBLE pnt, DOUBLE vel, INT acc);
INT API spl_line_move1ax(INT ax1, pDOUBLE pnt, DOUBLE vel, INT acc);
INT API spl_auto_line_move1ax(INT ax1, pDOUBLE pnt, DOUBLE vel, INT acc, INT auto_flag);

INT API position_compare_interval(INT dir, INT ax, INT bitNo, DOUBLE startpos, DOUBLE limitpos, LONG interval, LONG time);
INT API set_control_timer_ax(INT ax, DOUBLE time);
INT API fset_control_timer_ax(INT ax, DOUBLE time);
INT API get_control_timer_ax(INT ax, pDOUBLE time);
INT API fget_control_timer_ax(INT ax, pDOUBLE time);

INT API position_io_onoff(INT pos_num, INT bitNo, INT ax, DOUBLE pos, INT encflag);
INT API position_io_allclear(INT ax);
INT API position_io_clear(INT ax, INT pos_num);

INT API set_friction_gain(INT ax, LONG gain);
INT API fset_friction_gain(INT ax, LONG gain);
INT API get_friction_gain(INT ax, pLONG gain);
INT API fget_friction_gain(INT ax, pLONG gain);
INT API set_friction_range(INT axno, DOUBLE range);
INT API fset_friction_range(INT axno, DOUBLE range);
INT API get_friction_range(INT axno, pDOUBLE range);
INT API fget_friction_range(INT axno, pDOUBLE range);

INT API set_system_io(INT ax, INT onoff);
INT API get_system_io(INT ax, pINT onoff);
INT API set_amp_resolution32(INT ax, LONG resolution);
INT API fset_amp_resolution32(INT ax, LONG resolution);
INT API get_amp_resolution32(INT ax, pLONG resolution);
INT API fget_amp_resolution32(INT ax, pLONG resolution);

INT API set_collision_prevent_ax(INT ax, INT enable, INT slave_ax, INT add_sub, INT non_equal, DOUBLE c_pos, INT type);
INT API get_collision_prevent_ax(INT ax, pINT enable);

INT API set_sync_control_ax(INT ax, INT enable, INT master_ax, LONG gain);
INT API get_sync_control_ax(INT ax, pINT enable, pINT master_ax, pLONG gain);

INT API set_pause_control(INT bn, INT enable, LONG io_bit);
INT API get_collision_position(INT ax, pDOUBLE position);
INT API get_teachposition(INT ax, pDOUBLE position);

INT API set_jog_velocity(INT ax, INT JogPosIONum, INT JogNegIONum, DOUBLE jog_vel);
INT API fset_jog_velocity(INT ax, INT JogPosIONum, INT JogNegIONum, DOUBLE jog_vel);
INT API get_jog_velocity(pINT ax, pINT JogPosIONum, pINT JogNegIONum, pDOUBLE jog_vel);
INT API set_jog_enable(INT ax, INT state);
INT API get_jog_enable(INT ax, pINT state);

INT API set_mpg_velocity(INT ax,INT mpg_vel);
INT API get_mpg_velocity(INT ax,pINT mpg_vel);
INT API set_mpg_enable(INT ax, INT state);
INT API get_mpg_enable(INT ax, pINT state);

INT API set_enc_open_check_para(INT bdNum, INT analog_ref, INT count_ref, INT tolerance);
INT API get_enc_open_check_para(INT bdNum, pINT analog_ref, pINT count_ref, pINT tolerance);
INT API set_mpg_quarter_counter(INT ax,INT state);
// ver 1.32
INT API set_event_sharing_axes(INT ax1, INT ax2, INT state);
INT API get_event_sharing_axes(INT bdNum, pINT ax1, pINT ax2, pINT state);
// kckim 2009-07-06 Ver 1.33
INT API get_zphase_position(INT ax, pDOUBLE pos);
// ytlim 2009-08-04 Ver 1.40
INT API set_switch_status(INT ax, INT state);

INT API homing_start(INT ax);
INT API homing_stop(INT ax);
INT API homing_process(INT ax);
INT API set_homing_option(INT ax, ULONG Timeout, INT Config, INT Dir);
INT API get_homing_option(INT ax, pULONG pTimeout, pINT pConfig, pINT pDir);
INT API set_homing_profile(INT ax, DOUBLE MainVel, INT MainDec, DOUBLE RetVel, INT RetDec, 
								   DOUBLE FineVel, INT FineDec, DOUBLE MarkerVel, INT MarkerDec);
INT API get_homing_profile(INT ax, pDOUBLE pMainVel, pINT pMainDec, pDOUBLE pRetVel, pINT pRetDec, 
								   pDOUBLE pFineVel, pINT pFineDec, pDOUBLE pMarkerVel, pINT pMarkerDec);
// Library Test : 2010.08
INT API mmcsw_version_chk(pINT);
INT API set_mmcsw_version(INT);
INT API	AxisPowerOnCheck(INT);

#ifdef __cplusplus
}
#endif