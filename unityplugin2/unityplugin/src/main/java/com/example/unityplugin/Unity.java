package com.example.unityplugin;

import android.Manifest;
import android.content.Intent;
import android.content.pm.PackageManager;
import android.os.Bundle;
import android.support.v13.app.ActivityCompat;
import android.support.v4.content.ContextCompat;

import com.unity3d.player.UnityPlayerNativeActivity;

/**
 * Created by kimjin on 2017-06-30.
 */

public class Unity extends UnityPlayerNativeActivity {

    Intent intent = getIntent();
    UnityCamPreview m_unity = (UnityCamPreview)intent.getSerializableExtra("m_unity");
    private static final int PERMISSION_REQUEST_CAMERA = 100;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);

        int permissionCheck = ContextCompat.checkSelfPermission(this.getApplicationContext(), Manifest.permission.CAMERA);
        if (permissionCheck != PackageManager.PERMISSION_GRANTED)
        {
            ActivityCompat.requestPermissions(this, new String[]{Manifest.permission.CAMERA}, PERMISSION_REQUEST_CAMERA);
        }
    }

    @Override
    protected void onResume()
    {
        super.onResume();



    }


    public int get_texture_width()
    {
        return m_unity.get_texture_width();
    }

    public int get_texture_height()
    {
        return m_unity.get_texture_height();
    }

    public int get_data_length()
    {
        return m_unity.get_data_length();
    }

    public int get_M_start_stat()
    {
        return m_unity.get_M_start_stat();
    }

    public String teststring()
    {
        return m_unity.getTeststring();
    }
}
