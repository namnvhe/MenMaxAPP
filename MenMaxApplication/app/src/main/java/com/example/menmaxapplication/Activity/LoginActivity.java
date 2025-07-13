package com.example.menmaxapplication.Activity;

import android.content.Intent;
import android.os.Bundle;
import android.text.TextUtils;
import android.util.Log;
import android.view.View;
import android.widget.Button;
import android.widget.EditText;
import android.widget.Toast;

import androidx.activity.EdgeToEdge;
import androidx.appcompat.app.AppCompatActivity;
import androidx.core.graphics.Insets;
import androidx.core.view.ViewCompat;
import androidx.core.view.WindowInsetsCompat;

import com.example.menmaxapplication.Model.Address;
import com.example.menmaxapplication.Model.User;
import com.example.menmaxapplication.R;
import com.example.menmaxapplication.Retrofit.UserAPI;
import com.example.menmaxapplication.Somethings.ObjectSharedPreferences;

import retrofit2.Call;
import retrofit2.Response;

public class LoginActivity extends AppCompatActivity {

    private EditText edtUserName;
    private EditText edtPassWord;
    User user = new User();
    private Button btnLogin;

    private void bindingView(){
        edtUserName = findViewById(R.id.edtUserName);
        edtPassWord = findViewById(R.id.edtPassword);
        btnLogin = findViewById(R.id.btnLogin);
    }
    private void bindingAction(){
        btnLogin.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                Login();
            }
        });

    }

    private void Login() {
        edtPassWord = findViewById(R.id.edtPassword);
        edtUserName = findViewById(R.id.edtUserName);
        if (TextUtils.isEmpty(edtUserName.getText().toString())){
            edtUserName.setError("Please enter your username");
            edtUserName.requestFocus();
            return;
        }

        if (TextUtils.isEmpty(edtPassWord.getText().toString())){
            edtPassWord.setError("Please enter your password");
            edtPassWord.requestFocus();
            return;
        }
        String username = edtUserName.getText().toString();
        String password = edtPassWord.getText().toString();

        UserAPI.userApi.Login(username,password).enqueue(new retrofit2.Callback<User>() {
            @Override
            public void onResponse(Call<User> call, Response<User> response) {

                user = response.body();
                if (user!=null){
                    Toast.makeText(LoginActivity.this,"Login Successfully", Toast.LENGTH_LONG).show();

                    ObjectSharedPreferences.saveObjectToSharedPreference(LoginActivity.this, "User", "MODE_PRIVATE", user);
                    if(user.getAddress()!=null && user.getPhone_Number()!=null){
                        Address address = new Address(user.getUser_Name(), user.getPhone_Number(), user.getAddress());
                        ObjectSharedPreferences.saveObjectToSharedPreference(LoginActivity.this, "address", "MODE_PRIVATE", address);
                    }

                    Intent intent= new Intent(LoginActivity.this, MainActivity.class);
                    intent.putExtra("object", user);
                    startActivity(intent);
                    finish();
                }
                else{
                    Toast.makeText(LoginActivity.this,"Incorrect UserName or Password", Toast.LENGTH_LONG).show();
                }
                Log.e("ffff", "Đăng nhập thành công");
            }

            @Override
            public void onFailure(Call<User> call, Throwable t) {
                Toast.makeText(LoginActivity.this,"Failed to connect, try again later", Toast.LENGTH_LONG).show();
                Log.e("ffff", "Kết nối API Login thất bại");
                Log.e("TAG", t.toString());
            }
        });
    }

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        EdgeToEdge.enable(this);
        setContentView(R.layout.activity_login);
        ViewCompat.setOnApplyWindowInsetsListener(findViewById(R.id.main), (v, insets) -> {
            Insets systemBars = insets.getInsets(WindowInsetsCompat.Type.systemBars());
            v.setPadding(systemBars.left, systemBars.top, systemBars.right, systemBars.bottom);
            return insets;
        });
        bindingView();
        bindingAction();
    }


}