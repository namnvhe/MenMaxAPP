package com.example.menmaxapp.Retrofit;

import com.example.menmaxapp.Model.Cart;

import java.util.List;

import retrofit2.Call;
import retrofit2.http.Field;
import retrofit2.http.FormUrlEncoded;
import retrofit2.http.GET;
import retrofit2.http.POST;
import retrofit2.http.Query;

public interface CartAPI {


    RetrofitService retrofitService = new RetrofitService();
    CartAPI cartAPI = retrofitService.getRetrofit().create(CartAPI.class);

    @FormUrlEncoded
    @POST("/api/Cart/addtocart")
    Call<Cart> addToCart(@Field("user_id") String user_id, @Field("product_id") int product_id, @Field("count") int count);


    @GET("/api/Cart/cartofuser")
    Call<List<Cart>> cartOfUser(@Query("userId") String userId);

    @FormUrlEncoded
    @POST("/api/Cart/deletecart")
    Call<String> deleteCart(@Field("cart_id") int cart_id, @Field("user_id") String user_id);

}
