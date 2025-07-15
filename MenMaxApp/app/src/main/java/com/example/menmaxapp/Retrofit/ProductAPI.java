package com.example.menmaxapp.Retrofit;

import com.example.menmaxapp.Model.Product;

import java.util.List;

import retrofit2.Call;
import retrofit2.http.GET;
import retrofit2.http.Query;

public interface ProductAPI {

    RetrofitService retrofitService = new RetrofitService();
    ProductAPI productApi = retrofitService.getRetrofit().create(ProductAPI.class);
    @GET("/api/Product/newproduct")
    Call<List<Product>> getNewProduct();

    @GET("/api/Product/bestsellers")
    Call<List<Product>> getBestSellers();
    //    @FormUrlEncoded
    @GET("/api/Product/search")
    Call<List<Product>> search(@Query("searchContent") String searchContent);

}
