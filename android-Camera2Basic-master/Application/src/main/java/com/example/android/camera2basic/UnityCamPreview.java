package com.example.android.camera2basic;

/**
 * Created by kimjin on 2017-06-27.
 */

public class UnityCamPreview{
    private byte[] m_image;
    private int m_Yb, m_Ub, m_Vb;
    private int m_width;
    private int m_height;
    private int m_data_length;
    private int m_start_app = 99;
    private String test;

    public void set_image_buffer (byte[] image, int Yb, int Ub, int Vb, int datalength)
    {
        m_image = image;
        m_Yb = Yb;
        m_Ub = Ub;
        m_Vb = Vb;
        m_data_length = datalength;
    }

    public void set_texture_width(int width)
    {
        m_width = width;
    }

    public void set_texture_height(int height)
    {
        m_height = height;
    }

    public byte[] get_image ()
    {
        return m_image;
    }

    public int get_Yb()
    {
        return m_Yb;
    }

    public int get_Ub()
    {
        return m_Ub;
    }

    public int get_Vb()
    {
        return m_Vb;
    }

    public int get_texture_width()
    {
        return m_width;
    }

    public int get_texture_height()
    {
        return m_height;
    }

    public int get_data_length()
    {
        return m_data_length;
    }

    public void start_cam()
    {
        m_start_app = 1;
    }

    public void end_cam()
    {
        m_start_app = 96;
    }

    public int get_M_start_stat()
    {
        return m_start_app;
    }

    public String getTeststring()
    {
        test = "ABCDEFG";
        return test;
    }
}

