package com.example.menmaxapp.Adapter;

import android.content.Context;
import android.content.Intent;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.ImageView;
import android.widget.TextView;
import android.widget.Toast;

import androidx.annotation.NonNull;
import androidx.recyclerview.widget.RecyclerView;

import com.bumptech.glide.Glide;
import com.example.menmaxapp.Activity.ShowDetailActivity;
import com.example.menmaxapp.Model.Cart;
import com.example.menmaxapp.Model.Product;
import com.example.menmaxapp.Model.User;
import com.example.menmaxapp.R;
import com.example.menmaxapp.Retrofit.CartAPI;
import com.example.menmaxapp.Somethings.ObjectSharedPreferences;

import java.text.NumberFormat;
import java.util.ArrayList;
import java.util.List;
import java.util.Locale;

import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;

public class ProductAdapter extends RecyclerView.Adapter<ProductAdapter.ViewHolder> {
    private List<Product> products; // Use lowercase for variable naming (convention)
    private Context context;

    // Constructor with null check
    public ProductAdapter(List<Product> products, Context context) {
        this.products = (products != null) ? products : new ArrayList<>(); // Initialize with empty list if null
        this.context = context;
    }

    // Method to update data dynamically
    public void updateData(List<Product> newProducts) {
        this.products = (newProducts != null) ? newProducts : new ArrayList<>();
        notifyDataSetChanged();
    }

    @NonNull
    @Override
    public ViewHolder onCreateViewHolder(@NonNull ViewGroup parent, int viewType) {
        View inflate = LayoutInflater.from(parent.getContext()).inflate(R.layout.viewholder_products, parent, false);
        return new ViewHolder(inflate);
    }

    @Override
    public void onBindViewHolder(@NonNull ViewHolder holder, int position) {
        Product product = products.get(position);
        if (product == null) {
            return; // Prevent null product access
        }

        // Set product name
        holder.title.setText(product.getProduct_Name() != null ? product.getProduct_Name() : "N/A");

        // Format price
        Locale localeEN = new Locale("en", "EN");
        NumberFormat en = NumberFormat.getInstance(localeEN);
        holder.fee.setText(product.getPrice() > 0 ? en.format(product.getPrice()) : "N/A");

        // Load image with Glide, with error handling
        List<?> productImages = product.getProductImage();
        String imageUrl = (productImages != null && !productImages.isEmpty()) ? product.getProductImage().get(0).getUrl_Image() : null;
        Glide.with(holder.itemView.getContext())
                .load(imageUrl)
                /*.placeholder(R.drawable.placeholder) // Add placeholder image
                .error(R.drawable.error_image) // Add error image*/
                .into(holder.pic);

        // Add to cart button click
        holder.addBtn.setOnClickListener(v -> {
            User user = ObjectSharedPreferences.getSavedObjectFromPreference(context, "User", "MODE_PRIVATE", User.class);
            if (user == null) {
                Toast.makeText(context, "Please log in to add to cart", Toast.LENGTH_SHORT).show();
                return;
            }

            CartAPI.cartAPI.addToCart(user.getId(), product.getId(), 1).enqueue(new Callback<Cart>() {
                @Override
                public void onResponse(Call<Cart> call, Response<Cart> response) {
                    Cart cart = response.body();
                    Toast.makeText(context, cart != null ? "Thêm vào giỏ thành công" : "Thêm vào giỏ thất bại", Toast.LENGTH_SHORT).show();
                }

                @Override
                public void onFailure(Call<Cart> call, Throwable t) {
                    Toast.makeText(context, "Call API Add to cart failed: " + t.getMessage(), Toast.LENGTH_SHORT).show();
                }
            });
        });

        // Item click to show details
        holder.itemView.setOnClickListener(v -> {
            Intent intent = new Intent(holder.itemView.getContext(), ShowDetailActivity.class);
            intent.putExtra("product", product);
            holder.itemView.getContext().startActivity(intent);
        });
    }

    @Override
    public int getItemCount() {
        return products != null ? products.size() : 0; // Safe null check
    }

    public static class ViewHolder extends RecyclerView.ViewHolder {
        TextView title, fee;
        ImageView pic, addBtn;

        public ViewHolder(@NonNull View itemView) {
            super(itemView);
            title = itemView.findViewById(R.id.title);
            pic = itemView.findViewById(R.id.ivImage);
            fee = itemView.findViewById(R.id.fee);
            addBtn = itemView.findViewById(R.id.addBtn);
        }
    }
}