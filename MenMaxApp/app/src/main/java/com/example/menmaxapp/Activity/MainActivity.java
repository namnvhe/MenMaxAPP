package com.example.menmaxapp.Activity;

import androidx.appcompat.app.AppCompatActivity;
import androidx.recyclerview.widget.LinearLayoutManager;
import androidx.recyclerview.widget.RecyclerView;

import android.content.Intent;
import android.content.SharedPreferences;
import android.os.Bundle;
import android.util.Log;
import android.view.KeyEvent;
import android.view.View;
import android.widget.EditText;
import android.widget.ImageView;
import android.widget.TextView;

import com.bumptech.glide.Glide;
import com.example.menmaxapp.Adapter.CategoryAdapter;
import com.example.menmaxapp.Adapter.ProductAdapter;
import com.example.menmaxapp.Model.Category;
import com.example.menmaxapp.Model.Product;
import com.example.menmaxapp.Model.User;
import com.example.menmaxapp.R;
import com.example.menmaxapp.Retrofit.CategoryAPI;
import com.example.menmaxapp.Retrofit.ProductAPI;
import com.example.menmaxapp.Somethings.ObjectSharedPreferences;

import java.util.List;

import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;

public class MainActivity extends AppCompatActivity {
    SharedPreferences sharedpreferences;
    private RecyclerView.Adapter adapter, adapter2, adapter3;

    private RecyclerView recyclerViewCategoryList, recyclerViewNewProductList, recyclerViewBestSellersList;
    TextView tvHiName, tvSeeAll, tvSeeAll2;

    EditText etSearch;

    ImageView ivAvatar, ivHome, ivUser, ivCart, ivHistory, ivSearch;
    User user;

    //Api

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);
        AnhXa();
        appBarClick();
        LoadUserInfor();
        LoadCategories();
        LoadNewProducts();
        LoadBestSellers();
        ivSearchClick();
        seeAllClick();
        seeAllClick2();
    }

    private void seeAllClick() {
        tvSeeAll.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                Intent intent = new Intent(MainActivity.this, ProductsActivity.class);
                intent.putExtra("category_id", "-1"); // Use -1 to indicate all products
                intent.putExtra("searchContent", ""); // Empty search to show all products
                startActivity(intent);
            }
        });
    }

    private void seeAllClick2() {
        tvSeeAll2.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                Intent intent = new Intent(MainActivity.this, ProductsActivity.class);
                intent.putExtra("category_id", "-1"); // Use -1 to indicate all products
                intent.putExtra("searchContent", ""); // Empty search to show all products
                startActivity(intent);
            }
        });
    }

    private void ivSearchClick() {
        ivSearch.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                Intent intent = new Intent(MainActivity.this, ProductsActivity.class);
                intent.putExtra("searchContent", etSearch.getText().toString());
                startActivity(intent);
            }
        });

        etSearch.setOnKeyListener(new View.OnKeyListener() {
            public boolean onKey(View v, int keyCode, KeyEvent event) {
                // If the event is a key-down event on the "enter" button
                if ((event.getAction() == KeyEvent.ACTION_DOWN) &&
                        (keyCode == KeyEvent.KEYCODE_ENTER)) {
                    // Perform action on key press
                    Intent intent = new Intent(MainActivity.this, ProductsActivity.class);
                    intent.putExtra("searchContent", etSearch.getText().toString());
                    intent.putExtra("category_id", "-1");
                    startActivity(intent);
                    return true;
                }
                return false;
            }
        });
    }

    private void appBarClick() {
        ivHome.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                startActivity(new Intent(MainActivity.this, MainActivity.class));
                finish();
            }
        });
        ivUser.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                startActivity(new Intent(MainActivity.this, UserActivity.class));
                finish();
            }
        });
        ivCart.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                startActivity(new Intent(MainActivity.this, CartActivity.class));
                finish();
            }
        });

        ivHistory.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                startActivity(new Intent(MainActivity.this, OrderActivity.class));
                finish();
            }
        });


}

    private void LoadBestSellers() {
        LinearLayoutManager linearLayoutManager = new LinearLayoutManager(this, LinearLayoutManager.HORIZONTAL,false);
        recyclerViewBestSellersList = findViewById(R.id.view3);
        recyclerViewBestSellersList.setLayoutManager(linearLayoutManager);

        //GET API
        ProductAPI.productApi.getBestSellers().enqueue(new Callback<List<Product>>() {
            @Override
            public void onResponse(Call<List<Product>> call, Response<List<Product>> response) {
                List<Product> newProductsList = response.body();
                adapter3 = new ProductAdapter(newProductsList, MainActivity.this);
                recyclerViewBestSellersList.setAdapter(adapter3);
            }

            @Override
            public void onFailure(Call<List<Product>> call, Throwable t) {
                Log.e("====", "Call API Get Best Sellers fail");
            }
        });
    }

    private void LoadNewProducts() {
        LinearLayoutManager linearLayoutManager = new LinearLayoutManager(this, LinearLayoutManager.HORIZONTAL,false);
        recyclerViewNewProductList = findViewById(R.id.view2);
        recyclerViewNewProductList.setLayoutManager(linearLayoutManager);

        //GET API
        ProductAPI.productApi.getNewProduct().enqueue(new Callback<List<Product>>() {
            @Override
            public void onResponse(Call<List<Product>> call, Response<List<Product>> response) {
                List<Product> newProductsList = response.body();
                adapter2 = new ProductAdapter(newProductsList, MainActivity.this);
                recyclerViewNewProductList.setAdapter(adapter2);
            }

            @Override
            public void onFailure(Call<List<Product>> call, Throwable t) {
                Log.e("++++", t.getMessage());
                Log.e("====", "Call API Get New Products fail");
            }
        });
    }

    private void LoadCategories() {
        LinearLayoutManager linearLayoutManager = new LinearLayoutManager(this, LinearLayoutManager.HORIZONTAL, false);
        recyclerViewCategoryList = findViewById(R.id.view1);
        recyclerViewCategoryList.setLayoutManager(linearLayoutManager);
        //Get API
        CategoryAPI.categoryAPI.GetAllCategories().enqueue(new Callback<List<Category>>() {
            @Override
            public void onResponse(Call<List<Category>> call, Response<List<Category>> response) {
                List<Category> categoriesList = response.body();
                adapter = new CategoryAdapter(categoriesList, MainActivity.this);
                recyclerViewCategoryList.setAdapter(adapter);
            }

            @Override
            public void onFailure(Call<List<Category>> call, Throwable t) {
                Log.e("====", "Call API Get Categories fail");
            }
        });
    }

    private void LoadUserInfor() {
        user = ObjectSharedPreferences.getSavedObjectFromPreference(MainActivity.this, "User", "MODE_PRIVATE", User.class);
        tvHiName.setText("Xin chào "+ user.getUser_Name());
        Glide.with(getApplicationContext()).load(user.getAvatar()).into(ivAvatar);
    }

    private void AnhXa() {
        tvHiName = findViewById(R.id.tvHiName);
        ivAvatar = findViewById(R.id.ivAvatar);
        ivHome = findViewById(R.id.ivHome);
        ivUser = findViewById(R.id.ivUser);
        ivCart = findViewById(R.id.ivCart);
        ivHistory = findViewById(R.id.ivHistory);
        etSearch = findViewById(R.id.etSearch);
        ivSearch = findViewById(R.id.ivSearch);
        tvSeeAll = findViewById(R.id.tvSeeAll); // Bind tvSeeAll
        tvSeeAll2 = findViewById(R.id.tvSeeAll2); // Bind tvSeeAll2
    }
}