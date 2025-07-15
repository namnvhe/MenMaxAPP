package com.example.menmaxapp.Retrofit;

import com.example.menmaxapp.Model.Category;

import java.util.List;

import retrofit2.Call;
import retrofit2.http.GET;

public interface CategoryAPI {
    RetrofitService retrofitService = new RetrofitService();
    CategoryAPI categoryAPI = retrofitService.getRetrofit().create(CategoryAPI.class);
    @GET("/api/Category/category")
    Call<List<Category>> GetAllCategories();
}
