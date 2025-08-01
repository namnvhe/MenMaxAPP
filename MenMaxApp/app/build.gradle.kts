plugins {
    id("com.android.application")
}

android {
    namespace = "com.example.menmaxapp"
    compileSdk = 33

    defaultConfig {
        applicationId = "com.example.menmaxapp"
        minSdk = 21
        targetSdk = 33
        versionCode = 1
        versionName = "1.0"

        testInstrumentationRunner = "androidx.test.runner.AndroidJUnitRunner"
    }

    buildTypes {
        release {
            isMinifyEnabled = false
            proguardFiles(getDefaultProguardFile("proguard-android-optimize.txt"), "proguard-rules.pro")
        }
    }
    compileOptions {
        sourceCompatibility = JavaVersion.VERSION_1_8
        targetCompatibility = JavaVersion.VERSION_1_8
    }
}

dependencies {

    implementation("androidx.appcompat:appcompat:1.6.1")
    implementation("com.google.android.material:material:1.8.0")
    implementation("androidx.constraintlayout:constraintlayout:2.1.4")
    implementation(fileTree(mapOf("dir" to "C:\\Users\\Admin\\AndroidStudioProjects\\MenMaxApp\\zalopay", "include" to listOf("*.aar", "*.jar"), "exclude" to emptyList<String>())))
    testImplementation("junit:junit:4.13.2")
    androidTestImplementation("androidx.test.ext:junit:1.1.5")
    androidTestImplementation("androidx.test.espresso:espresso-core:3.5.1")
    configurations.maybeCreate("default")
    artifacts.add("default", file("zpdk-release.aar"))
    //========================================================================
    implementation("com.google.android.material:material:1.4.0-rc01")
    implementation("com.squareup.retrofit2:retrofit:2.9.0")
    implementation("com.squareup.retrofit2:converter-gson:2.9.0")
    implementation("com.squareup.retrofit2:converter-scalars:2.9.0")
    implementation("com.squareup.okhttp3:logging-interceptor:3.12.0")

    //Gson
    implementation("com.google.code.gson:gson:2.10.1")
    //thu viện load image
    implementation("com.github.bumptech.glide:glide:4.14.2")
    annotationProcessor("com.github.bumptech.glide:compiler:4.14.2")
    //thu viện load dữ liệu API
    implementation("com.android.volley:volley:1.2.1")
    //thư viện circle images
    //bo goc tron cho ImageView
    implementation("de.hdodenhof:circleimageview:3.1.0")
    implementation("com.github.smarteist:Android-Image-Slider:1.4.0")
    //
    implementation("com.github.chthai64:SwipeRevealLayout:1.4.0")
    //zalopay
    implementation("com.squareup.okhttp3:okhttp:4.6.0")
    implementation("commons-codec:commons-codec:1.14")
    //SmsConfirmationView
    implementation("com.github.fraggjkee:sms-confirmation-view:1.7.1")

    // Dependency for Google Sign-In
    implementation("com.google.android.gms:play-services-auth:20.5.0")
}
