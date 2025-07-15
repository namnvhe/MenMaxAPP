package com.example.menmaxapp.Activity;

import android.app.Dialog;
import android.app.ProgressDialog;
import android.content.Intent;
import android.graphics.Color;
import android.graphics.drawable.ColorDrawable;
import android.os.Bundle;
import android.text.TextUtils;
import android.util.Log;
import android.view.Gravity;
import android.view.View;
import android.view.ViewGroup;
import android.widget.Button;
import android.widget.EditText;
import android.widget.ImageView;
import android.widget.TextView;
import android.widget.Toast;

import androidx.appcompat.app.AppCompatActivity;
import androidx.constraintlayout.widget.ConstraintLayout;

import com.example.menmaxapp.R;
import com.example.menmaxapp.Retrofit.UserAPI;
import com.fraggjkee.smsconfirmationview.SmsConfirmationView;

import java.util.Objects;

import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;

public class ForgotPasswordActivity extends AppCompatActivity {
    EditText etUserName, etNewPass, etReNewPass;
    Button btnSubmit, btnSubmitVerification, btnSubmitPassword;
    ConstraintLayout clForgotPassword, clVerification, clSetNewPassword;
    SmsConfirmationView smsConfirmationView;
    TextView tvLogin1, tvLogin2, tvLogin3, tvUserNameNotCorrect, tvCodeNotCorrect, tvPasswordNotMatch;
    ImageView ivBack;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_forgot_password);
        AnhXa();
        btnSubmitClick();
        tvLogin1Click();
    }

    private void tvLogin1Click() {
        if (tvLogin1 != null) {
            tvLogin1.setOnClickListener(v -> {
                startActivity(new Intent(ForgotPasswordActivity.this, LoginActivity.class));
            });
        }
    }

    private void btnSubmitClick() {
        if (btnSubmit != null) {
            btnSubmit.setOnClickListener(view -> {
                String user_id = getTextSafely(etUserName);
                if (TextUtils.isEmpty(user_id)) {
                    Toast.makeText(this, "Please enter your user name!", Toast.LENGTH_SHORT).show();
                    if (etUserName != null) {
                        etUserName.requestFocus();
                    }
                    return;
                }

                ProgressDialog progressDialog = new ProgressDialog(ForgotPasswordActivity.this);
                progressDialog.setMessage("Checking..."); // Setting Message
                progressDialog.setTitle("Forgot Password"); // Setting Title
                progressDialog.setProgressStyle(ProgressDialog.STYLE_SPINNER); // Progress Dialog Style Spinner
                progressDialog.show(); // Display Progress Dialog
                progressDialog.setCancelable(false);

                UserAPI.userApi.forgotPassword(user_id).enqueue(new Callback<String>() {
                    @Override
                    public void onResponse(Call<String> call, Response<String> response) {
                        progressDialog.dismiss();

                        if (response.isSuccessful()) {
                            String codeForgot = response.body();

                            // Kiểm tra codeForgot không null
                            if (!TextUtils.isEmpty(codeForgot)) {
                                Log.e("===", codeForgot);

                                if (clForgotPassword != null) {
                                    clForgotPassword.setVisibility(View.GONE);
                                }
                                if (clVerification != null) {
                                    clVerification.setVisibility(View.VISIBLE);
                                }

                                setupSmsConfirmationView(codeForgot, user_id);
                                setupBackButton();
                                setupLoginButton2();
                            } else {
                                showError("Có lỗi xảy ra, vui lòng thử lại!");
                            }
                        } else {
                            showUsernameError();
                        }
                    }

                    @Override
                    public void onFailure(Call<String> call, Throwable t) {
                        String errorMessage = (t != null && t.getMessage() != null) ?
                                t.getMessage() : "Unknown error occurred";
                        Log.e("===++", errorMessage);
                        progressDialog.dismiss();
                        showError("Có lỗi kết nối, vui lòng thử lại!");
                    }
                });
            });
        }
    }

    private void setupSmsConfirmationView(String codeForgot, String user_id) {
        if (smsConfirmationView != null) {
            smsConfirmationView.setOnChangeListener((code, isComplete) -> {
                if (isComplete) {
                    if (!TextUtils.isEmpty(code) && !TextUtils.isEmpty(codeForgot) &&
                            Objects.equals(code, codeForgot)) {

                        if (clVerification != null) {
                            clVerification.setVisibility(View.GONE);
                        }
                        if (clSetNewPassword != null) {
                            clSetNewPassword.setVisibility(View.VISIBLE);
                        }

                        setupSubmitPasswordButton(user_id, codeForgot);
                        setupLoginButton3();
                    } else {
                        showCodeError();
                    }
                }
            });
        }
    }

    private void setupSubmitPasswordButton(String user_id, String codeForgot) {
        if (btnSubmitPassword != null) {
            btnSubmitPassword.setOnClickListener(v -> {
                String newPass = getTextSafely(etNewPass);
                String reNewPass = getTextSafely(etReNewPass);

                if (TextUtils.isEmpty(newPass) || TextUtils.isEmpty(reNewPass)) {
                    showError("Vui lòng nhập đầy đủ mật khẩu!");
                    return;
                }

                if (Objects.equals(newPass, reNewPass)) {
                    UserAPI.userApi.forgotNewPass(user_id, codeForgot, newPass).enqueue(new Callback<String>() {
                        @Override
                        public void onResponse(Call<String> call, Response<String> response) {
                            if (response.isSuccessful()) {
                                showSuccessDialog();
                            } else {
                                showError("Có lỗi xảy ra khi đổi mật khẩu!");
                            }
                        }

                        @Override
                        public void onFailure(Call<String> call, Throwable t) {
                            String errorMessage = (t != null && t.getMessage() != null) ?
                                    t.getMessage() : "Unknown error occurred";
                            Log.e("ChangePassword", errorMessage);
                            showError("Có lỗi kết nối, vui lòng thử lại!");
                        }
                    });
                } else {
                    showPasswordNotMatchError();
                }
            });
        }
    }

    private void setupBackButton() {
        if (ivBack != null) {
            ivBack.setOnClickListener(v -> {
                if (tvUserNameNotCorrect != null) {
                    tvUserNameNotCorrect.setVisibility(View.GONE);
                }
                if (etUserName != null) {
                    etUserName.setText("");
                }
                if (clVerification != null) {
                    clVerification.setVisibility(View.GONE);
                }
                if (clForgotPassword != null) {
                    clForgotPassword.setVisibility(View.VISIBLE);
                }
            });
        }
    }

    private void setupLoginButton2() {
        if (tvLogin2 != null) {
            tvLogin2.setOnClickListener(v -> {
                startActivity(new Intent(ForgotPasswordActivity.this, LoginActivity.class));
            });
        }
    }

    private void setupLoginButton3() {
        if (tvLogin3 != null) {
            tvLogin3.setOnClickListener(v -> {
                startActivity(new Intent(ForgotPasswordActivity.this, LoginActivity.class));
            });
        }
    }

    private void showSuccessDialog() {
        try {
            Dialog dialog = new Dialog(ForgotPasswordActivity.this);
            dialog.setContentView(R.layout.dialog_forgot_password_success);
            dialog.getWindow().setLayout(ViewGroup.LayoutParams.MATCH_PARENT, ViewGroup.LayoutParams.WRAP_CONTENT);
            dialog.getWindow().getAttributes().windowAnimations = R.style.animation;
            dialog.getWindow().setBackgroundDrawable(new ColorDrawable(Color.TRANSPARENT));
            dialog.getWindow().setGravity(Gravity.BOTTOM);
            dialog.setCancelable(false);
            dialog.show();

            Button btnLogin = dialog.findViewById(R.id.btnChangePassword);
            if (btnLogin != null) {
                btnLogin.setOnClickListener(v1 -> {
                    dialog.dismiss();
                    startActivity(new Intent(ForgotPasswordActivity.this, LoginActivity.class));
                    finish();
                });
            }
        } catch (Exception e) {
            Log.e("Dialog", "Error showing success dialog: " + e.getMessage());
            // Fallback: chuyển trực tiếp về LoginActivity
            startActivity(new Intent(ForgotPasswordActivity.this, LoginActivity.class));
            finish();
        }
    }

    private void showUsernameError() {
        if (tvUserNameNotCorrect != null) {
            tvUserNameNotCorrect.setVisibility(View.VISIBLE);
        }
        Toast.makeText(ForgotPasswordActivity.this, "Your username is not correct!", Toast.LENGTH_SHORT).show();
    }

    private void showCodeError() {
        if (tvCodeNotCorrect != null) {
            tvCodeNotCorrect.setVisibility(View.VISIBLE);
        }
        Toast.makeText(ForgotPasswordActivity.this, "Your code is not correct!", Toast.LENGTH_SHORT).show();
    }

    private void showPasswordNotMatchError() {
        if (tvPasswordNotMatch != null) {
            tvPasswordNotMatch.setVisibility(View.VISIBLE);
        }
        Toast.makeText(ForgotPasswordActivity.this, "Password and confirm password do not match!", Toast.LENGTH_SHORT).show();
    }

    private void showError(String message) {
        Toast.makeText(ForgotPasswordActivity.this, message, Toast.LENGTH_SHORT).show();
    }

    // Phương thức helper để lấy text an toàn
    private String getTextSafely(EditText editText) {
        if (editText != null && editText.getText() != null) {
            return editText.getText().toString().trim();
        }
        return "";
    }

    private void AnhXa() {
        try {
            // Anh xa clForgotPassword
            etUserName = findViewById(R.id.etUserName);
            btnSubmit = findViewById(R.id.btnSubmit);
            clForgotPassword = findViewById(R.id.clForgotPassword);
            clVerification = findViewById(R.id.clVerification);
            clSetNewPassword = findViewById(R.id.clSetNewPassword);
            tvLogin1 = findViewById(R.id.tvLogin1);
            tvUserNameNotCorrect = findViewById(R.id.tvUserNameNotCorrect);

            // Anh xa clVerification
            btnSubmitVerification = findViewById(R.id.btnSubmitVerification);
            smsConfirmationView = findViewById(R.id.smsCodeView);
            tvLogin2 = findViewById(R.id.tvLogin2);
            ivBack = findViewById(R.id.ivBack);
            tvCodeNotCorrect = findViewById(R.id.tvCodeNotCorrect);

            // Anh xa clSetNewPassword
            etNewPass = findViewById(R.id.etNewPass);
            etReNewPass = findViewById(R.id.etReNewPass);
            btnSubmitPassword = findViewById(R.id.btnSubmitPassword);
            tvLogin3 = findViewById(R.id.tvLogin3);
            tvPasswordNotMatch = findViewById(R.id.tvPasswordNotMatch);
        } catch (Exception e) {
            Log.e("AnhXa", "Error in findViewById: " + e.getMessage());
        }
    }
}
