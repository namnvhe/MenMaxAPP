USE [master]
GO
/****** Object:  Database [MenMax]    Script Date: 15/7/2025 11:21:06 PM ******/
CREATE DATABASE [MenMax]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'MenMax', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.SQLEXPRESS\MSSQL\DATA\MenMax.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'MenMax_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.SQLEXPRESS\MSSQL\DATA\MenMax_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT, LEDGER = OFF
GO
ALTER DATABASE [MenMax] SET COMPATIBILITY_LEVEL = 160
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [MenMax].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [MenMax] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [MenMax] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [MenMax] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [MenMax] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [MenMax] SET ARITHABORT OFF 
GO
ALTER DATABASE [MenMax] SET AUTO_CLOSE ON 
GO
ALTER DATABASE [MenMax] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [MenMax] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [MenMax] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [MenMax] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [MenMax] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [MenMax] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [MenMax] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [MenMax] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [MenMax] SET  ENABLE_BROKER 
GO
ALTER DATABASE [MenMax] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [MenMax] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [MenMax] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [MenMax] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [MenMax] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [MenMax] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [MenMax] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [MenMax] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [MenMax] SET  MULTI_USER 
GO
ALTER DATABASE [MenMax] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [MenMax] SET DB_CHAINING OFF 
GO
ALTER DATABASE [MenMax] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [MenMax] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [MenMax] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [MenMax] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
ALTER DATABASE [MenMax] SET QUERY_STORE = ON
GO
ALTER DATABASE [MenMax] SET QUERY_STORE (OPERATION_MODE = READ_WRITE, CLEANUP_POLICY = (STALE_QUERY_THRESHOLD_DAYS = 30), DATA_FLUSH_INTERVAL_SECONDS = 900, INTERVAL_LENGTH_MINUTES = 60, MAX_STORAGE_SIZE_MB = 1000, QUERY_CAPTURE_MODE = AUTO, SIZE_BASED_CLEANUP_MODE = AUTO, MAX_PLANS_PER_QUERY = 200, WAIT_STATS_CAPTURE_MODE = ON)
GO
USE [MenMax]
GO
/****** Object:  Table [dbo].[cart]    Script Date: 15/7/2025 11:21:06 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[cart](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[count] [int] NOT NULL,
	[product_id] [int] NULL,
	[user_id] [nvarchar](255) NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[category]    Script Date: 15/7/2025 11:21:06 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[category](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[category_name] [nvarchar](1111) NULL,
	[category_Image] [nvarchar](1111) NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[order]    Script Date: 15/7/2025 11:21:06 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[order](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[address] [nvarchar](1111) NULL,
	[booking_date] [date] NULL,
	[country] [nvarchar](1111) NULL,
	[email] [nvarchar](1111) NULL,
	[fullname] [nvarchar](1111) NULL,
	[note] [nvarchar](1111) NULL,
	[payment_method] [nvarchar](1111) NULL,
	[phone] [nvarchar](1111) NULL,
	[status] [nvarchar](1111) NULL,
	[total] [int] NULL,
	[user_id] [nvarchar](255) NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[order_item]    Script Date: 15/7/2025 11:21:06 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[order_item](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[count] [int] NULL,
	[order_id] [int] NULL,
	[product_id] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[product]    Script Date: 15/7/2025 11:21:06 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[product](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[created_at] [date] NULL,
	[description] [nvarchar](max) NULL,
	[is_active] [int] NULL,
	[is_selling] [int] NULL,
	[price] [int] NULL,
	[product_name] [nvarchar](1111) NULL,
	[quantity] [int] NULL,
	[sold] [int] NULL,
	[category_id] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[product_image]    Script Date: 15/7/2025 11:21:06 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[product_image](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[url_image] [nvarchar](1111) NULL,
	[product_id] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[user]    Script Date: 15/7/2025 11:21:06 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[user](
	[id] [nvarchar](255) NOT NULL,
	[avatar] [nvarchar](1111) NULL,
	[email] [nvarchar](1111) NULL,
	[login_type] [nvarchar](1111) NULL,
	[password] [nvarchar](1111) NULL,
	[phone_number] [nvarchar](1111) NULL,
	[role] [nvarchar](1111) NULL,
	[user_name] [nvarchar](1111) NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[cart] ON 

INSERT [dbo].[cart] ([id], [count], [product_id], [user_id]) VALUES (1, 5, 8, N'user7')
INSERT [dbo].[cart] ([id], [count], [product_id], [user_id]) VALUES (2, 4, 1, N'user3')
INSERT [dbo].[cart] ([id], [count], [product_id], [user_id]) VALUES (4, 1, 4, N'user15')
INSERT [dbo].[cart] ([id], [count], [product_id], [user_id]) VALUES (6, 3, 14, N'user7')
INSERT [dbo].[cart] ([id], [count], [product_id], [user_id]) VALUES (7, 4, 4, N'user2')
INSERT [dbo].[cart] ([id], [count], [product_id], [user_id]) VALUES (8, 2, 4, N'user9')
INSERT [dbo].[cart] ([id], [count], [product_id], [user_id]) VALUES (9, 3, 17, N'user10')
INSERT [dbo].[cart] ([id], [count], [product_id], [user_id]) VALUES (10, 5, 9, N'user14')
INSERT [dbo].[cart] ([id], [count], [product_id], [user_id]) VALUES (11, 3, 20, N'user18')
INSERT [dbo].[cart] ([id], [count], [product_id], [user_id]) VALUES (12, 3, 14, N'user8')
INSERT [dbo].[cart] ([id], [count], [product_id], [user_id]) VALUES (14, 3, 14, N'user16')
INSERT [dbo].[cart] ([id], [count], [product_id], [user_id]) VALUES (15, 4, 17, N'user5')
INSERT [dbo].[cart] ([id], [count], [product_id], [user_id]) VALUES (16, 2, 11, N'user2')
INSERT [dbo].[cart] ([id], [count], [product_id], [user_id]) VALUES (17, 2, 10, N'user4')
INSERT [dbo].[cart] ([id], [count], [product_id], [user_id]) VALUES (18, 4, 20, N'user14')
INSERT [dbo].[cart] ([id], [count], [product_id], [user_id]) VALUES (19, 4, 9, N'user6')
INSERT [dbo].[cart] ([id], [count], [product_id], [user_id]) VALUES (20, 3, 10, N'user10')
SET IDENTITY_INSERT [dbo].[cart] OFF
GO
SET IDENTITY_INSERT [dbo].[category] ON 

INSERT [dbo].[category] ([id], [category_name], [category_Image]) VALUES (1, N'Tăng Cường Sinh Lực', N'https://login.medlatec.vn//ImagePath/images/20220719/20220719_bai-tap-tang-cuong-sinh-ly-nam-gioi-1.jpg')
INSERT [dbo].[category] ([id], [category_name], [category_Image]) VALUES (2, N'Hỗ Trợ Testosteron', N'https://suckhoedoisong.qltns.mediacdn.vn/Images/nguyenkhanh/2017/12/17/testosteron.jpg')
INSERT [dbo].[category] ([id], [category_name], [category_Image]) VALUES (3, N'Thảo Dược Thiên Nhiên', N'https://production-cdn.pharmacity.io/digital/original/plain/blog/1f046e27716295dce29c7dca7fc42b111747024037.jpg?X-Amz-Content-Sha256=UNSIGNED-PAYLOAD&X-Amz-Algorithm=AWS4-HMAC-SHA256&X-Amz-Credential=AKIAUYXZVMJM5QUYWSVO%2F20250612%2Fap-southeast-1%2Fs3%2Faws4_request&X-Amz-Date=20250612T044607Z&X-Amz-SignedHeaders=host&X-Amz-Expires=600&X-Amz-Signature=4bfa7f1bf57fe14c58276d4b05909b9aaa056d6a7ac80a137fc41318094e460d')
INSERT [dbo].[category] ([id], [category_name], [category_Image]) VALUES (4, N'Hỗ Trợ Sinh Lý', N'https://suckhoedoisong.qltns.mediacdn.vn/324455921873985536/2022/9/8/photo-1662634227999-1662634228148449542887.jpg')
INSERT [dbo].[category] ([id], [category_name], [category_Image]) VALUES (5, N'Kéo Dài Thời Gian', N'https://aceman.vn/wp-content/uploads/2022/09/an-gi-keo-dai-thoi-gian-quan-he.jpg')
INSERT [dbo].[category] ([id], [category_name], [category_Image]) VALUES (6, N'Vitamin & Khoáng Chất', N'https://suckhoedoisong.qltns.mediacdn.vn/Images/nguyenkhanh/2020/09/10/vitamin-va-khoang-chat-dung-the-nao-cho-an-toan1599710061.jpg')
INSERT [dbo].[category] ([id], [category_name], [category_Image]) VALUES (7, N'Cải Thiện Sinh Lực', N'https://thanhtrangpharma.vn/uploads/1%20tang%20cuong%20sinh%20ly%20nam%20gioi/tang%20cuong%20sinh%20ly%20nam%20gioi2.jpg')
INSERT [dbo].[category] ([id], [category_name], [category_Image]) VALUES (8, N'Thảo Dược Tự Nhiên', N'https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcSH-WAz0yNEC6qx1P8ANGtH-L0PTZf2FHLqIw&s')
INSERT [dbo].[category] ([id], [category_name], [category_Image]) VALUES (9, N'Tăng Cường Sinh Lý', N'https://cdn.nhathuoclongchau.com.vn/unsafe/https://cms-prod.s3-sgn09.fptcloud.com/5_bai_tap_tang_cuong_sinh_ly_cho_nam_gioi_hieu_qua_2_2646909505.jpg')
INSERT [dbo].[category] ([id], [category_name], [category_Image]) VALUES (10, N'Tăng Cường Ham Muốn', N'https://nld.mediacdn.vn/zoom/594_371/2020/11/26/4biquyetkhienchangnoidienvibankhienchangsaydam11601612400136width763height467ztbq-16063708821421202589583.jpg')
INSERT [dbo].[category] ([id], [category_name], [category_Image]) VALUES (11, N'Hỗ Trợ Tiền Liệt Tuyến', N'https://cdn.nhathuoclongchau.com.vn/unsafe/https://cms-prod.s3-sgn09.fptcloud.com/su_that_hieu_qua_cua_thuoc_bo_tinh_trung_danh_cho_nam_gioi_4_3c096f8662.jpg')
INSERT [dbo].[category] ([id], [category_name], [category_Image]) VALUES (12, N'Dinh Dưỡng', N'https://suckhoedoisong.qltns.mediacdn.vn/324455921873985536/2021/8/18/tong-quan-ve-he-nang-luong-va-dinh-duong-16292763896801667506521.jpg')
INSERT [dbo].[category] ([id], [category_name], [category_Image]) VALUES (13, N'Kiểm Soát Xuất Tinh Sớm', N'https://suckhoedoisong.qltns.mediacdn.vn/324455921873985536/2023/1/3/xuat-tinh-som-16727309454831523234425.jpg')
INSERT [dbo].[category] ([id], [category_name], [category_Image]) VALUES (14, N'Giải Độc Cơ Thể', N'https://baoquangbinh.vn/dataimages/201405/original/images569190_uongnuoc40308362.jpg')
INSERT [dbo].[category] ([id], [category_name], [category_Image]) VALUES (15, N'Bổ thận tráng dương', N'https://medlatec.vn/media/31736/file/thuoc-bo-tinh-trung-4.jpg')
INSERT [dbo].[category] ([id], [category_name], [category_Image]) VALUES (16, N'Gel Bôi Ngoài', N'https://cdn.nhathuoclongchau.com.vn/unsafe/800x0/https://cms-prod.s3-sgn09.fptcloud.com/Giai_dap_thac_mac_tinh_trung_khoe_nhat_vao_thoi_gian_nao_trong_ngay_3_90787f9588.jpg')
INSERT [dbo].[category] ([id], [category_name], [category_Image]) VALUES (17, N'Tăng Cường Sinh Lực', N'https://login.medlatec.vn//ImagePath/images/20220530/20220530_tang-chat-luong-tinh-trung-3.png')
INSERT [dbo].[category] ([id], [category_name], [category_Image]) VALUES (18, N'Nâng Cao Sức Khoẻ', N'https://www.vinmec.com/static/uploads/large_20200512_090007_854063_phinh_dong_mach_chu_max_1800x1800_jpg_8194d96cad.jpg')
INSERT [dbo].[category] ([id], [category_name], [category_Image]) VALUES (19, N'Sức Khỏe Tim Mạch Nam', N'https://www.vinmec.com/static/uploads/small_20191015_092730_666337_phau_thuat_giam_can_max_1800x1800_jpg_2b2c4277b0.jpg')
INSERT [dbo].[category] ([id], [category_name], [category_Image]) VALUES (20, N'Tăng Cường Đề Kháng', N'https://ivfhongngoc.com/wp-content/uploads/2021/05/cao-huyet-ap-la-gi-9-900x471.jpg')
SET IDENTITY_INSERT [dbo].[category] OFF
GO
SET IDENTITY_INSERT [dbo].[order] ON 

INSERT [dbo].[order] ([id], [address], [booking_date], [country], [email], [fullname], [note], [payment_method], [phone], [status], [total], [user_id]) VALUES (1, N'Số 1, Ngõ 123, Đường Láng, Đống Đa, Hà Nội', CAST(N'2024-11-05' AS Date), N'Vietnam', N'nguyenvanan@gmail.com', N'Nguyễn Văn An', N'Giao hàng kín đáo', N'paypal', N'0775116606', N'completed', 605654, N'user7')
INSERT [dbo].[order] ([id], [address], [booking_date], [country], [email], [fullname], [note], [payment_method], [phone], [status], [total], [user_id]) VALUES (2, N'Số 2, Phố Hàng Bài, Hoàn Kiếm, Hà Nội', CAST(N'2024-04-25' AS Date), N'Vietnam', N'tranhongduc@yahoo.com', N'Trần Hồng Đức', N'Không gọi điện trước khi giao', N'paypal', N'0519176489', N'completed', 589928, N'user16')
INSERT [dbo].[order] ([id], [address], [booking_date], [country], [email], [fullname], [note], [payment_method], [phone], [status], [total], [user_id]) VALUES (3, N'Số 3, Đường Kim Mã, Ba Đình, Hà Nội', CAST(N'2024-12-03' AS Date), N'Vietnam', N'tranquocgia@hotmail.com', N'Trần Quốc Gia', N'Để ở cổng, không bấm chuông', N'cash', N'0447941797', N'completed', 580092, N'user7')
INSERT [dbo].[order] ([id], [address], [booking_date], [country], [email], [fullname], [note], [payment_method], [phone], [status], [total], [user_id]) VALUES (4, N'Số 4, Ngõ Thổ Quan, Khâm Thiên, Đống Đa, Hà Nội', CAST(N'2024-10-27' AS Date), N'Vietnam', N'ngotienem@yahoo.com', N'Ngô Tiến Em', N'Giao sau 6h tối', N'paypal', N'0727609444', N'completed', 855207, N'user5')
INSERT [dbo].[order] ([id], [address], [booking_date], [country], [email], [fullname], [note], [payment_method], [phone], [status], [total], [user_id]) VALUES (5, N'Số 5, Đường Giải Phóng, Hai Bà Trưng, Hà Nội', CAST(N'2024-05-08' AS Date), N'Vietnam', N'tranquocgia@hotmail.com', N'Trần Quốc Gia', N'Liên hệ khi đến', N'paypal', N'0302004653', N'pending', 404776, N'user7')
INSERT [dbo].[order] ([id], [address], [booking_date], [country], [email], [fullname], [note], [payment_method], [phone], [status], [total], [user_id]) VALUES (6, N'Số 6, Đường Trần Duy Hưng, Cầu Giấy, Hà Nội', CAST(N'2025-04-17' AS Date), N'Vietnam', N'phamlambac@gmail.com', N'Phạm Lâm Bắc', N'Đóng gói kín đáo', N'credit card', N'0640946287', N'completed', 413957, N'user18')
INSERT [dbo].[order] ([id], [address], [booking_date], [country], [email], [fullname], [note], [payment_method], [phone], [status], [total], [user_id]) VALUES (7, N'Số 7, Phố Huế, Hai Bà Trưng, Hà Nội', CAST(N'2024-06-25' AS Date), N'Vietnam', N'nguyenvietcuong@example.com', N'Nguyễn Việt Cường', N'Giao tận tay', N'paypal', N'0249516149', N'completed', 153816, N'user20')
INSERT [dbo].[order] ([id], [address], [booking_date], [country], [email], [fullname], [note], [payment_method], [phone], [status], [total], [user_id]) VALUES (8, N'Số 8, Đường Phạm Văn Đồng, Cầu Giấy, Hà Nội', CAST(N'2023-01-19' AS Date), N'Vietnam', N'nguyenvanan@gmail.com', N'Nguyễn Văn An', N'Không ghi tên sản phẩm lên gói hàng', N'paypal', N'0262386916', N'pending', 763633, N'user1')
INSERT [dbo].[order] ([id], [address], [booking_date], [country], [email], [fullname], [note], [payment_method], [phone], [status], [total], [user_id]) VALUES (9, N'Số 9, Đường Lạc Long Quân, Tây Hồ, Hà Nội', CAST(N'2025-04-17' AS Date), N'Vietnam', N'tranvanbinh@example.com', N'Trần Văn Bình', N'Liên hệ số này nếu có vấn đề: 0987654321', N'paypal', N'0730392789', N'pending', 809407, N'user2')
INSERT [dbo].[order] ([id], [address], [booking_date], [country], [email], [fullname], [note], [payment_method], [phone], [status], [total], [user_id]) VALUES (10, N'Số 10, Đường Hoàng Hoa Thám, Ba Đình, Hà Nội', CAST(N'2024-05-07' AS Date), N'Vietnam', N'dohuythang@hotmail.com', N'Đỗ Huy Thắng', N'Chỉ giao khi có người nhận', N'credit card', N'0915882867', N'cancelled', 483572, N'user14')
INSERT [dbo].[order] ([id], [address], [booking_date], [country], [email], [fullname], [note], [payment_method], [phone], [status], [total], [user_id]) VALUES (11, N'Số 11, Ngõ 100, Đường Nguyễn Trãi, Thanh Xuân, Hà Nội', CAST(N'2024-05-01' AS Date), N'Vietnam', N'ngotienem@yahoo.com', N'Ngô Tiến Em', N'Đặt hàng lần 2, giao nhanh giúp', N'paypal', N'0695243771', N'cancelled', 573340, N'user5')
INSERT [dbo].[order] ([id], [address], [booking_date], [country], [email], [fullname], [note], [payment_method], [phone], [status], [total], [user_id]) VALUES (12, N'Số 12, Đường Thái Hà, Đống Đa, Hà Nội', CAST(N'2024-10-23' AS Date), N'Vietnam', N'phamlambac@gmail.com', N'Phạm Lâm Bắc', N'Gói cẩn thận, không móp méo', N'cash', N'0811663092', N'completed', 351111, N'user18')
INSERT [dbo].[order] ([id], [address], [booking_date], [country], [email], [fullname], [note], [payment_method], [phone], [status], [total], [user_id]) VALUES (13, N'Số 13, Đường Bạch Mai, Hai Bà Trưng, Hà Nội', CAST(N'2024-03-18' AS Date), N'Vietnam', N'dongthanhhai@gmail.com', N'Đồng Thanh Hải', N'Giao vào buổi sáng', N'credit card', N'0456337061', N'cancelled', 647217, N'user19')
INSERT [dbo].[order] ([id], [address], [booking_date], [country], [email], [fullname], [note], [payment_method], [phone], [status], [total], [user_id]) VALUES (14, N'Số 14, Đường Lê Duẩn, Hoàn Kiếm, Hà Nội', CAST(N'2025-10-16' AS Date), N'Vietnam', N'tranhongduc@yahoo.com', N'Trần Hồng Đức', N'Hàng cần gấp', N'cash', N'0325593453', N'completed', 544015, N'user16')
INSERT [dbo].[order] ([id], [address], [booking_date], [country], [email], [fullname], [note], [payment_method], [phone], [status], [total], [user_id]) VALUES (15, N'Số 15, Đường Cầu Giấy, Cầu Giấy, Hà Nội', CAST(N'2024-10-03' AS Date), N'Vietnam', N'ngotienem@yahoo.com', N'Ngô Tiến Em', N'Ưu tiên giao hàng nhanh', N'credit card', N'0393028349', N'pending', 257631, N'user5')
INSERT [dbo].[order] ([id], [address], [booking_date], [country], [email], [fullname], [note], [payment_method], [phone], [status], [total], [user_id]) VALUES (16, N'Số 16, Đường Trường Chinh, Thanh Xuân, Hà Nội', CAST(N'2024-09-10' AS Date), N'Vietnam', N'buiduclinh@gmail.com', N'Bùi Đức Linh', N'Đóng gói riêng tư', N'paypal', N'0671387209', N'completed', 940690, N'user9')
INSERT [dbo].[order] ([id], [address], [booking_date], [country], [email], [fullname], [note], [payment_method], [phone], [status], [total], [user_id]) VALUES (17, N'Số 17, Đường Tây Sơn, Đống Đa, Hà Nội', CAST(N'2024-05-14' AS Date), N'Vietnam', N'nguyenxuanhieu@example.com', N'Nguyễn Xuân Hiếu', N'Không báo trước khi giao', N'cash', N'0003130638', N'completed', 718556, N'user15')
INSERT [dbo].[order] ([id], [address], [booking_date], [country], [email], [fullname], [note], [payment_method], [phone], [status], [total], [user_id]) VALUES (18, N'Số 18, Đường Khuất Duy Tiến, Thanh Xuân, Hà Nội', CAST(N'2023-08-20' AS Date), N'Vietnam', N'dohuythang@hotmail.com', N'Đỗ Huy Thắng', N'Nhận vào giờ hành chính', N'paypal', N'0175258117', N'pending', 347951, N'user14')
INSERT [dbo].[order] ([id], [address], [booking_date], [country], [email], [fullname], [note], [payment_method], [phone], [status], [total], [user_id]) VALUES (19, N'Số 19, Đường Nguyễn Chí Thanh, Đống Đa, Hà Nội', CAST(N'2024-11-08' AS Date), N'Vietnam', N'tranquocgia@hotmail.com', N'Trần Quốc Gia', N'Liên hệ qua Zalo', N'cash', N'0354715174', N'pending', 209813, N'user7')
INSERT [dbo].[order] ([id], [address], [booking_date], [country], [email], [fullname], [note], [payment_method], [phone], [status], [total], [user_id]) VALUES (20, N'Số 20, Đường Kim Giang, Thanh Xuân, Hà Nội', CAST(N'2023-11-21' AS Date), N'Vietnam', N'vuvankhang@gmail.com', N'Vũ Văn Khang', N'Giao vào cuối tuần', N'paypal', N'0624071953', N'cancelled', 415233, N'user8')
INSERT [dbo].[order] ([id], [address], [booking_date], [country], [email], [fullname], [note], [payment_method], [phone], [status], [total], [user_id]) VALUES (21, N'HN', CAST(N'2025-07-14' AS Date), N'Việt Nam', N'nguyenvanan@gmail.com', N'Hoang', NULL, N'Pay on Delivery', N'0987654321', N'Pending', 518134, N'user1')
INSERT [dbo].[order] ([id], [address], [booking_date], [country], [email], [fullname], [note], [payment_method], [phone], [status], [total], [user_id]) VALUES (22, N'HN', CAST(N'2025-07-15' AS Date), N'Việt Nam', N'hoangnhhe170662@fpt.edu.vn', N'Hoàng Nguyễn Huy', NULL, N'Pay on Delivery', N'0987654321', N'Pending', 300000, N'user1')
SET IDENTITY_INSERT [dbo].[order] OFF
GO
SET IDENTITY_INSERT [dbo].[order_item] ON 

INSERT [dbo].[order_item] ([id], [count], [order_id], [product_id]) VALUES (1, 2, 20, 8)
INSERT [dbo].[order_item] ([id], [count], [order_id], [product_id]) VALUES (2, 5, 13, 10)
INSERT [dbo].[order_item] ([id], [count], [order_id], [product_id]) VALUES (3, 4, 14, 19)
INSERT [dbo].[order_item] ([id], [count], [order_id], [product_id]) VALUES (4, 4, 10, 13)
INSERT [dbo].[order_item] ([id], [count], [order_id], [product_id]) VALUES (5, 2, 11, 19)
INSERT [dbo].[order_item] ([id], [count], [order_id], [product_id]) VALUES (6, 4, 10, 13)
INSERT [dbo].[order_item] ([id], [count], [order_id], [product_id]) VALUES (7, 4, 10, 20)
INSERT [dbo].[order_item] ([id], [count], [order_id], [product_id]) VALUES (8, 5, 16, 11)
INSERT [dbo].[order_item] ([id], [count], [order_id], [product_id]) VALUES (9, 2, 13, 14)
INSERT [dbo].[order_item] ([id], [count], [order_id], [product_id]) VALUES (10, 3, 13, 9)
INSERT [dbo].[order_item] ([id], [count], [order_id], [product_id]) VALUES (11, 1, 19, 5)
INSERT [dbo].[order_item] ([id], [count], [order_id], [product_id]) VALUES (12, 3, 1, 16)
INSERT [dbo].[order_item] ([id], [count], [order_id], [product_id]) VALUES (13, 1, 3, 7)
INSERT [dbo].[order_item] ([id], [count], [order_id], [product_id]) VALUES (14, 5, 15, 15)
INSERT [dbo].[order_item] ([id], [count], [order_id], [product_id]) VALUES (15, 3, 9, 8)
INSERT [dbo].[order_item] ([id], [count], [order_id], [product_id]) VALUES (16, 2, 17, 7)
INSERT [dbo].[order_item] ([id], [count], [order_id], [product_id]) VALUES (17, 3, 3, 4)
INSERT [dbo].[order_item] ([id], [count], [order_id], [product_id]) VALUES (18, 1, 10, 12)
INSERT [dbo].[order_item] ([id], [count], [order_id], [product_id]) VALUES (19, 4, 6, 18)
INSERT [dbo].[order_item] ([id], [count], [order_id], [product_id]) VALUES (20, 5, 11, 3)
INSERT [dbo].[order_item] ([id], [count], [order_id], [product_id]) VALUES (21, 1, 21, 3)
INSERT [dbo].[order_item] ([id], [count], [order_id], [product_id]) VALUES (22, 1, 22, 3)
SET IDENTITY_INSERT [dbo].[order_item] OFF
GO
SET IDENTITY_INSERT [dbo].[product] ON 

INSERT [dbo].[product] ([id], [created_at], [description], [is_active], [is_selling], [price], [product_name], [quantity], [sold], [category_id]) VALUES (1, CAST(N'2024-05-14' AS Date), N'Viên uống hỗ trợ cương cứng và tăng ham muốn hiệu quả.', 1, 1, 615000, N'Ironmen Ocavill', 70, 7, 9)
INSERT [dbo].[product] ([id], [created_at], [description], [is_active], [is_selling], [price], [product_name], [quantity], [sold], [category_id]) VALUES (2, CAST(N'2024-04-17' AS Date), N'Công thức đặc biệt hỗ trợ sản xuất Testosteron tự nhiên.', 1, 1, 530000, N'Bang Cevrai', 45, 50, 2)
INSERT [dbo].[product] ([id], [created_at], [description], [is_active], [is_selling], [price], [product_name], [quantity], [sold], [category_id]) VALUES (3, CAST(N'2025-08-10' AS Date), N'Thảo dược quý hiếm giúp bổ thận, tráng dương, tăng cường sức khỏe.', 1, 1, 300000, N'KingsUp', 46, 2, 3)
INSERT [dbo].[product] ([id], [created_at], [description], [is_active], [is_selling], [price], [product_name], [quantity], [sold], [category_id]) VALUES (4, CAST(N'2024-10-08' AS Date), N'Cải thiện chất lượng và số lượng tinh trùng, tăng khả năng thụ thai.', 1, 1, 1200000, N'Best King Jpanwell', 61, 30, 7)
INSERT [dbo].[product] ([id], [created_at], [description], [is_active], [is_selling], [price], [product_name], [quantity], [sold], [category_id]) VALUES (5, CAST(N'2024-04-03' AS Date), N'Gel bôi ngoài giúp kéo dài thời gian quan hệ, tự tin hơn.', 1, 1, 300000, N'Bix for Gentlemen', 52, 39, 5)
INSERT [dbo].[product] ([id], [created_at], [description], [is_active], [is_selling], [price], [product_name], [quantity], [sold], [category_id]) VALUES (6, CAST(N'2023-06-23' AS Date), N'Bổ sung vitamin và khoáng chất thiết yếu cho sức khỏe nam giới.', 1, 1, 350000, N'Immuvita Easylife', 89, 10, 6)
INSERT [dbo].[product] ([id], [created_at], [description], [is_active], [is_selling], [price], [product_name], [quantity], [sold], [category_id]) VALUES (7, CAST(N'2025-03-23' AS Date), N'Hỗ trợ tăng cường ham muốn và năng lượng tổng thể.', 1, 0, 700000, N'Men’s Ginseng Alipas Ecogreen', 84, 2, 10)
INSERT [dbo].[product] ([id], [created_at], [description], [is_active], [is_selling], [price], [product_name], [quantity], [sold], [category_id]) VALUES (8, CAST(N'2024-01-17' AS Date), N'Chiết xuất tự nhiên giúp duy trì sức khỏe tiền liệt tuyến.', 1, 1, 400000, N'Prostate Fort', 52, 11, 11)
INSERT [dbo].[product] ([id], [created_at], [description], [is_active], [is_selling], [price], [product_name], [quantity], [sold], [category_id]) VALUES (9, CAST(N'2024-09-06' AS Date), N'Giúp kiểm soát xuất tinh sớm, mang lại trải nghiệm tốt hơn.', 1, 0, 280000, N'Duraject-60', 15, 49, 13)
INSERT [dbo].[product] ([id], [created_at], [description], [is_active], [is_selling], [price], [product_name], [quantity], [sold], [category_id]) VALUES (10, CAST(N'2024-04-16' AS Date), N'Nước uống giúp thải độc, làm sạch cơ thể, hỗ trợ sức khỏe sinh lý.', 1, 1, 550000, N'Pure Cordyceps', 3, 27, 14)
INSERT [dbo].[product] ([id], [created_at], [description], [is_active], [is_selling], [price], [product_name], [quantity], [sold], [category_id]) VALUES (11, CAST(N'2024-02-03' AS Date), N'Thực phẩm bổ sung cho nam giới tập luyện, tăng cường sức bền.', 1, 0, 59000, N'Gel Liquid Energy', 16, 19, 12)
INSERT [dbo].[product] ([id], [created_at], [description], [is_active], [is_selling], [price], [product_name], [quantity], [sold], [category_id]) VALUES (12, CAST(N'2024-11-04' AS Date), N'Viên uống tổng hợp giúp cải thiện sức khỏe tổng thể và sinh lý.', 1, 0, 715000, N'Essence Of Kangaroo', 80, 39, 18)
INSERT [dbo].[product] ([id], [created_at], [description], [is_active], [is_selling], [price], [product_name], [quantity], [sold], [category_id]) VALUES (13, CAST(N'2025-04-03' AS Date), N'Hỗ trợ sức khỏe tim mạch, gián tiếp cải thiện lưu thông máu.', 1, 0, 437000, N'Heart Ace Support', 77, 29, 19)
INSERT [dbo].[product] ([id], [created_at], [description], [is_active], [is_selling], [price], [product_name], [quantity], [sold], [category_id]) VALUES (14, CAST(N'2024-06-21' AS Date), N'Hỗ trợ tăng cường sinh lực, tăng cường đề kháng cho nam giới ', 1, 1, 420000, N'Oyster Plus', 20, 26, 8)
INSERT [dbo].[product] ([id], [created_at], [description], [is_active], [is_selling], [price], [product_name], [quantity], [sold], [category_id]) VALUES (15, CAST(N'2024-04-11' AS Date), N'Sản phẩm từ thảo dược giúp bổ thận tráng dương.', 1, 1, 298000, N'Viganam', 52, 15, 15)
INSERT [dbo].[product] ([id], [created_at], [description], [is_active], [is_selling], [price], [product_name], [quantity], [sold], [category_id]) VALUES (16, CAST(N'2023-12-08' AS Date), N'Viên uống Linh Tự Đan Hồng Bàng giúp tăng cường sinh lý nam, hỗ trợ giảm nguy cơ mãn dục nam', 1, 0, 190000, N'Linh Tự Đan Hồng Bàng', 98, 9, 16)
INSERT [dbo].[product] ([id], [created_at], [description], [is_active], [is_selling], [price], [product_name], [quantity], [sold], [category_id]) VALUES (17, CAST(N'2023-05-09' AS Date), N'Rocket 1h Thái Dương giúp bổ thận dương, tăng cường sinh lực cho phái mạnh.', 1, 1, 300000, N'Rocket 1h', 87, 9, 17)
INSERT [dbo].[product] ([id], [created_at], [description], [is_active], [is_selling], [price], [product_name], [quantity], [sold], [category_id]) VALUES (18, CAST(N'2025-06-12' AS Date), N'Pure Cordyceps Pharmekal giúp tăng cường sức đề kháng của cơ thể', 1, 1, 550000, N'Pure Cordyceps', 91, 2, 20)
INSERT [dbo].[product] ([id], [created_at], [description], [is_active], [is_selling], [price], [product_name], [quantity], [sold], [category_id]) VALUES (19, CAST(N'2024-11-27' AS Date), N'Viên sủi tiện lợi tăng cường sức đề kháng cho cơ thể', 1, 1, 118000, N'Immunity Booster Vid-Fighter', 56, 12, 1)
INSERT [dbo].[product] ([id], [created_at], [description], [is_active], [is_selling], [price], [product_name], [quantity], [sold], [category_id]) VALUES (20, CAST(N'2024-11-26' AS Date), N' Tăng cường sinh lực nam giới.', 1, 1, 1000000, N'Proxerex Sigma', 46, 35, 4)
SET IDENTITY_INSERT [dbo].[product] OFF
GO
SET IDENTITY_INSERT [dbo].[product_image] ON 

INSERT [dbo].[product_image] ([id], [url_image], [product_id]) VALUES (1, N'https://cdn.nhathuoclongchau.com.vn/unsafe/768x0/filters:quality(90)/https://cms-prod.s3-sgn09.fptcloud.com/00501597_vien_uong_tang_cuong_sinh_ly_ironmen_ocavill_60v_5022_6302_large_3cb863cf94.jpg', 1)
INSERT [dbo].[product_image] ([id], [url_image], [product_id]) VALUES (2, N'https://cdn.nhathuoclongchau.com.vn/unsafe/768x0/filters:quality(90)/https://cms-prod.s3-sgn09.fptcloud.com/DSC_03083_8c550201f0.jpg', 2)
INSERT [dbo].[product_image] ([id], [url_image], [product_id]) VALUES (3, N'https://cdn.nhathuoclongchau.com.vn/unsafe/768x0/filters:quality(90)/https://cms-prod.s3-sgn09.fptcloud.com/DSC_02506_eb5bce023a.jpg', 3)
INSERT [dbo].[product_image] ([id], [url_image], [product_id]) VALUES (4, N'https://cdn.nhathuoclongchau.com.vn/unsafe/768x0/filters:quality(90)/https://cms-prod.s3-sgn09.fptcloud.com/00021931_best_king_jpanwell_60v_8243_5f92_large_ff343d4575.JPG', 4)
INSERT [dbo].[product_image] ([id], [url_image], [product_id]) VALUES (5, N'https://cdn.nhathuoclongchau.com.vn/unsafe/768x0/filters:quality(90)/https://cms-prod.s3-sgn09.fptcloud.com/00032916_gel_boi_tang_cuong_sinh_ly_cho_nam_gioi_bix_gentlement_30ml_4746_61aa_large_5deb02b34f.jpg', 5)
INSERT [dbo].[product_image] ([id], [url_image], [product_id]) VALUES (6, N'https://cdn.nhathuoclongchau.com.vn/unsafe/768x0/filters:quality(90)/https://cms-prod.s3-sgn09.fptcloud.com/DSC_05505_4c243a16f9.jpg', 6)
INSERT [dbo].[product_image] ([id], [url_image], [product_id]) VALUES (7, N'https://cdn.nhathuoclongchau.com.vn/unsafe/768x0/filters:quality(90)/https://cms-prod.s3-sgn09.fptcloud.com/00028815_alipas_new_ecogreen_30v_6727_5f99_large_4206f8f2d7.JPG', 7)
INSERT [dbo].[product_image] ([id], [url_image], [product_id]) VALUES (8, N'https://cdn.nhathuoclongchau.com.vn/unsafe/768x0/filters:quality(90)/https://cms-prod.s3-sgn09.fptcloud.com/00029880_prostate_fort_200mg_vitamins_for_life_30v_2149_6331_large_7ea35e64bc.jpg', 8)
INSERT [dbo].[product_image] ([id], [url_image], [product_id]) VALUES (9, N'https://cdn.nhathuoclongchau.com.vn/unsafe/768x0/filters:quality(90)/https://cms-prod.s3-sgn09.fptcloud.com/00503875_c35d8651d6.jpg', 9)
INSERT [dbo].[product_image] ([id], [url_image], [product_id]) VALUES (10, N'https://cdn.nhathuoclongchau.com.vn/unsafe/768x0/filters:quality(90)/https://cms-prod.s3-sgn09.fptcloud.com/00345341_dong_trung_ha_thao_pure_cordyceps_8634_5c34_large_81ed7c754c.jpg', 10)
INSERT [dbo].[product_image] ([id], [url_image], [product_id]) VALUES (11, N'https://cdn.nhathuoclongchau.com.vn/unsafe/768x0/filters:quality(90)/https://cms-prod.s3-sgn09.fptcloud.com/5_53d0b78d79.jpg', 11)
INSERT [dbo].[product_image] ([id], [url_image], [product_id]) VALUES (12, N'https://cdn.nhathuoclongchau.com.vn/unsafe/768x0/filters:quality(90)/https://cms-prod.s3-sgn09.fptcloud.com/00033668_vien_uong_tang_cuong_suc_khoe_nam_gioi_vitatree_essence_of_kangaroo_40000_max_100v_9977_623c_large_2f0749a6b2.jpg', 12)
INSERT [dbo].[product_image] ([id], [url_image], [product_id]) VALUES (13, N'https://cdn.nhathuoclongchau.com.vn/unsafe/768x0/filters:quality(90)/https://cms-prod.s3-sgn09.fptcloud.com/00033382_vien_uong_ho_tro_tim_mach_heart_ace_support_vitamins_for_life_30v_9384_63e1_large_9a7ec569a1.jpg', 13)
INSERT [dbo].[product_image] ([id], [url_image], [product_id]) VALUES (14, N'https://cdn.nhathuoclongchau.com.vn/unsafe/768x0/filters:quality(90)/https://cms-prod.s3-sgn09.fptcloud.com/00005685_tinh_chat_hau_oyster_plus_tang_cuong_sinh_luc_phai_manh_3213_62ae_large_c5942edd08.jpg', 14)
INSERT [dbo].[product_image] ([id], [url_image], [product_id]) VALUES (15, N'https://cdn.nhathuoclongchau.com.vn/unsafe/768x0/filters:quality(90)/https://cms-prod.s3-sgn09.fptcloud.com/00032791_viganam_tam_binh_hop_5_vi_x_12_vien_9291_62af_large_4689423966.jpg', 15)
INSERT [dbo].[product_image] ([id], [url_image], [product_id]) VALUES (16, N'https://cdn.nhathuoclongchau.com.vn/unsafe/768x0/filters:quality(90)/https://cms-prod.s3-sgn09.fptcloud.com/00004429_linh_tu_dan_ho_tro_dieu_tri_vo_sinh_hiem_muon_2219_62af_large_a3bcd4e156.jpg', 16)
INSERT [dbo].[product_image] ([id], [url_image], [product_id]) VALUES (17, N'https://cdn.nhathuoclongchau.com.vn/unsafe/768x0/filters:quality(90)/https://cms-prod.s3-sgn09.fptcloud.com/DSC_08500_e94b46dbd1.jpg', 17)
INSERT [dbo].[product_image] ([id], [url_image], [product_id]) VALUES (18, N'https://cdn.nhathuoclongchau.com.vn/unsafe/768x0/filters:quality(90)/https://cms-prod.s3-sgn09.fptcloud.com/00345341_dong_trung_ha_thao_pure_cordyceps_8634_5c34_large_81ed7c754c.jpg', 18)
INSERT [dbo].[product_image] ([id], [url_image], [product_id]) VALUES (19, N'https://cdn.nhathuoclongchau.com.vn/unsafe/768x0/filters:quality(90)/https://cms-prod.s3-sgn09.fptcloud.com/00033702_vien_uong_tang_suc_khang_optimax_immunity_booster_vid_fighter_20v_5588_62ae_large_eed24d39f3.jpg', 19)
INSERT [dbo].[product_image] ([id], [url_image], [product_id]) VALUES (20, N'https://cdn.nhathuoclongchau.com.vn/unsafe/636x0/filters:quality(90)/https://cms-prod.s3-sgn09.fptcloud.com/DSC_00384_9440bc0251.jpg', 20)
SET IDENTITY_INSERT [dbo].[product_image] OFF
GO
INSERT [dbo].[user] ([id], [avatar], [email], [login_type], [password], [phone_number], [role], [user_name]) VALUES (N'109033908082847185050', N'null', N'hoangnhhe170662@fpt.edu.vn', N'google', NULL, NULL, N'user', N'Nguyen Huy Hoang K17 - HL')
INSERT [dbo].[user] ([id], [avatar], [email], [login_type], [password], [phone_number], [role], [user_name]) VALUES (N'116729589213026391930', N'https://lh3.googleusercontent.com/a/ACg8ocI4L2CNgVe0xOF07sJ_dR0KA72MhNW7CmPBGTbWpaloz7ZSqjfe', N'huyhoang03tx@gmail.com', N'google', NULL, NULL, N'user', N'Hoàng Nguyễn Huy')
INSERT [dbo].[user] ([id], [avatar], [email], [login_type], [password], [phone_number], [role], [user_name]) VALUES (N'user1', N'https://haycafe.vn/wp-content/uploads/2022/02/Avatar-trang-den.png', N'hoangnhhe170662@fpt.edu.vn', N'email', N'MTIzNA==', N'0461088521', N'user', N'AnNguyen')
INSERT [dbo].[user] ([id], [avatar], [email], [login_type], [password], [phone_number], [role], [user_name]) VALUES (N'user10', N'https://haycafe.vn/wp-content/uploads/2022/02/Avatar-trang-den.png', N'nguyenhoangminh@example.com', N'email', N'MTIzNA==', N'0415528350', N'user', N'MinhNguyen')
INSERT [dbo].[user] ([id], [avatar], [email], [login_type], [password], [phone_number], [role], [user_name]) VALUES (N'user11', N'https://haycafe.vn/wp-content/uploads/2022/02/Avatar-trang-den.png', N'tranvannguyen@example.com', N'facebook', N'MTIzNA==', N'0937014736', N'user', N'NguyenTran')
INSERT [dbo].[user] ([id], [avatar], [email], [login_type], [password], [phone_number], [role], [user_name]) VALUES (N'user12', N'https://haycafe.vn/wp-content/uploads/2022/02/Avatar-trang-den.png', N'lethanhphong@gmail.com', N'facebook', N'MTIzNA==', N'0703385906', N'admin', N'PhongLe')
INSERT [dbo].[user] ([id], [avatar], [email], [login_type], [password], [phone_number], [role], [user_name]) VALUES (N'user13', N'https://haycafe.vn/wp-content/uploads/2022/02/Avatar-trang-den.png', N'phamquanghuy@hotmail.com', N'email', N'MTIzNA==', N'0489064139', N'user', N'HuyPham')
INSERT [dbo].[user] ([id], [avatar], [email], [login_type], [password], [phone_number], [role], [user_name]) VALUES (N'user14', N'https://haycafe.vn/wp-content/uploads/2022/02/Avatar-trang-den.png', N'dohuythang@hotmail.com', N'email', N'MTIzNA==', N'0625528764', N'admin', N'ThangDo')
INSERT [dbo].[user] ([id], [avatar], [email], [login_type], [password], [phone_number], [role], [user_name]) VALUES (N'user15', N'https://haycafe.vn/wp-content/uploads/2022/02/Avatar-trang-den.pnghttps://haycafe.vn/wp-content/uploads/2022/02/Avatar-trang-den.png', N'nguyenxuanhieu@example.com', N'google', N'MTIzNA==', N'0176868683', N'user', N'HieuNguyen')
INSERT [dbo].[user] ([id], [avatar], [email], [login_type], [password], [phone_number], [role], [user_name]) VALUES (N'user16', N'https://haycafe.vn/wp-content/uploads/2022/02/Avatar-trang-den.png', N'tranhongduc@yahoo.com', N'email', N'MTIzNA==', N'0873725986', N'user', N'DucTran')
INSERT [dbo].[user] ([id], [avatar], [email], [login_type], [password], [phone_number], [role], [user_name]) VALUES (N'user17', N'https://haycafe.vn/wp-content/uploads/2022/02/Avatar-trang-den.png', N'levietanh@hotmail.com', N'facebook', N'MTIzNA==', N'0955509724', N'admin', N'AnhLe')
INSERT [dbo].[user] ([id], [avatar], [email], [login_type], [password], [phone_number], [role], [user_name]) VALUES (N'user18', N'https://haycafe.vn/wp-content/uploads/2022/02/Avatar-trang-den.png', N'phamlambac@gmail.com', N'email', N'MTIzNA==', N'0875764693', N'admin', N'BacPham')
INSERT [dbo].[user] ([id], [avatar], [email], [login_type], [password], [phone_number], [role], [user_name]) VALUES (N'user19', N'https://haycafe.vn/wp-content/uploads/2022/02/Avatar-trang-den.png', N'dongthanhhai@gmail.com', N'facebook', N'MTIzNA==', N'0328235012', N'user', N'HaiDong')
INSERT [dbo].[user] ([id], [avatar], [email], [login_type], [password], [phone_number], [role], [user_name]) VALUES (N'user2', N'https://haycafe.vn/wp-content/uploads/2022/02/Avatar-trang-den.png', N'tranvanbinh@example.com', N'google', N'MTIzNA==', N'0416689857', N'admin', N'BinhTran')
INSERT [dbo].[user] ([id], [avatar], [email], [login_type], [password], [phone_number], [role], [user_name]) VALUES (N'user20', N'https://haycafe.vn/wp-content/uploads/2022/02/Avatar-trang-den.png', N'nguyenvietcuong@example.com', N'facebook', N'MTIzNA==', N'0629513687', N'user', N'CuongNguyen')
INSERT [dbo].[user] ([id], [avatar], [email], [login_type], [password], [phone_number], [role], [user_name]) VALUES (N'user3', N'https://haycafe.vn/wp-content/uploads/2022/02/Avatar-trang-den.png', N'lehoangcuong@example.com', N'email', N'MTIzNA==', N'0129676504', N'admin', N'CuongLe')
INSERT [dbo].[user] ([id], [avatar], [email], [login_type], [password], [phone_number], [role], [user_name]) VALUES (N'user4', N'https://haycafe.vn/wp-content/uploads/2022/02/Avatar-trang-den.png', N'phamducdung@hotmail.com', N'email', N'MTIzNA==', N'0598203748', N'admin', N'DungPham')
INSERT [dbo].[user] ([id], [avatar], [email], [login_type], [password], [phone_number], [role], [user_name]) VALUES (N'user5', N'https://haycafe.vn/wp-content/uploads/2022/02/Avatar-trang-den.png', N'ngotienem@yahoo.com', N'google', N'MTIzNA==', N'0045487877', N'user', N'EmNgo')
INSERT [dbo].[user] ([id], [avatar], [email], [login_type], [password], [phone_number], [role], [user_name]) VALUES (N'user6', N'https://haycafe.vn/wp-content/uploads/2022/02/Avatar-trang-den.png', N'dovanphong@hotmail.com', N'email', N'MTIzNA==', N'0732129768', N'admin', N'PhongDo')
INSERT [dbo].[user] ([id], [avatar], [email], [login_type], [password], [phone_number], [role], [user_name]) VALUES (N'user7', N'https://haycafe.vn/wp-content/uploads/2022/02/Avatar-trang-den.png', N'tranquocgia@hotmail.com', N'email', N'MTIzNA==', N'0663409349', N'admin', N'GiaTran')
INSERT [dbo].[user] ([id], [avatar], [email], [login_type], [password], [phone_number], [role], [user_name]) VALUES (N'user8', N'https://haycafe.vn/wp-content/uploads/2022/02/Avatar-trang-den.png', N'vuvankhang@gmail.com', N'email', N'MTIzNA==', N'0618618724', N'admin', N'KhangVu')
INSERT [dbo].[user] ([id], [avatar], [email], [login_type], [password], [phone_number], [role], [user_name]) VALUES (N'user9', N'https://haycafe.vn/wp-content/uploads/2022/02/Avatar-trang-den.png', N'buiduclinh@gmail.com', N'facebook', N'MTIzNA==', N'0267319692', N'admin', N'LinhBui')
GO
ALTER TABLE [dbo].[cart]  WITH CHECK ADD  CONSTRAINT [FK3d704slv66tw6x5hmbm6p2x3u] FOREIGN KEY([product_id])
REFERENCES [dbo].[product] ([id])
GO
ALTER TABLE [dbo].[cart] CHECK CONSTRAINT [FK3d704slv66tw6x5hmbm6p2x3u]
GO
ALTER TABLE [dbo].[cart]  WITH CHECK ADD  CONSTRAINT [FKl70asp4l4w0jmbm1tqyofho4o] FOREIGN KEY([user_id])
REFERENCES [dbo].[user] ([id])
GO
ALTER TABLE [dbo].[cart] CHECK CONSTRAINT [FKl70asp4l4w0jmbm1tqyofho4o]
GO
ALTER TABLE [dbo].[order]  WITH CHECK ADD  CONSTRAINT [FKcpl0mjoeqhxvgeeeq5piwpd3i] FOREIGN KEY([user_id])
REFERENCES [dbo].[user] ([id])
GO
ALTER TABLE [dbo].[order] CHECK CONSTRAINT [FKcpl0mjoeqhxvgeeeq5piwpd3i]
GO
ALTER TABLE [dbo].[order_item]  WITH CHECK ADD  CONSTRAINT [FK551losx9j75ss5d6bfsqvijna] FOREIGN KEY([product_id])
REFERENCES [dbo].[product] ([id])
GO
ALTER TABLE [dbo].[order_item] CHECK CONSTRAINT [FK551losx9j75ss5d6bfsqvijna]
GO
ALTER TABLE [dbo].[order_item]  WITH CHECK ADD  CONSTRAINT [FKs234mi6jususbx4b37k44cipy] FOREIGN KEY([order_id])
REFERENCES [dbo].[order] ([id])
GO
ALTER TABLE [dbo].[order_item] CHECK CONSTRAINT [FKs234mi6jususbx4b37k44cipy]
GO
ALTER TABLE [dbo].[product]  WITH CHECK ADD  CONSTRAINT [FK1mtsbur82frn64de7balymq9s] FOREIGN KEY([category_id])
REFERENCES [dbo].[category] ([id])
GO
ALTER TABLE [dbo].[product] CHECK CONSTRAINT [FK1mtsbur82frn64de7balymq9s]
GO
ALTER TABLE [dbo].[product_image]  WITH CHECK ADD  CONSTRAINT [FK6oo0cvcdtb6qmwsga468uuukk] FOREIGN KEY([product_id])
REFERENCES [dbo].[product] ([id])
GO
ALTER TABLE [dbo].[product_image] CHECK CONSTRAINT [FK6oo0cvcdtb6qmwsga468uuukk]
GO
USE [master]
GO
ALTER DATABASE [MenMax] SET  READ_WRITE 
GO
