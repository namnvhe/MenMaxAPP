-- DROP DATABASE MenMax;
CREATE DATABASE MenMax;
USE MenMax;

-- Tạo bảng
CREATE TABLE cart (
    id INT NOT NULL IDENTITY(1,1),
    count INT NOT NULL,
    product_id INT,
    user_id NVARCHAR(255),
    PRIMARY KEY (id)
);

CREATE TABLE category (
    id INT NOT NULL IDENTITY(1,1),
    category_name NVARCHAR(1111),
    category_Image NVARCHAR(1111),
    PRIMARY KEY (id)
);

CREATE TABLE [order] (
    id INT NOT NULL IDENTITY(1,1),
    address NVARCHAR(1111),
    booking_date DATE,
    country NVARCHAR(1111),
    email NVARCHAR(1111),
    fullname NVARCHAR(1111),
    note NVARCHAR(1111),
    payment_method NVARCHAR(1111),
    phone NVARCHAR(1111),
    status NVARCHAR(1111),
    total INT,
    user_id NVARCHAR(255),
    PRIMARY KEY (id)
);

CREATE TABLE order_item (
    id INT NOT NULL IDENTITY(1,1),
    count INT,
    order_id INT,
    product_id INT,
    PRIMARY KEY (id)
);

CREATE TABLE product (
    id INT NOT NULL IDENTITY(1,1),
    created_at DATE,
    description NVARCHAR(MAX),
    is_active INT,
    is_selling INT,
    price INT,
    product_name NVARCHAR(1111),
    quantity INT,
    sold INT,
    category_id INT,
    PRIMARY KEY (id)
);

CREATE TABLE product_image (
    id INT NOT NULL IDENTITY(1,1),
    url_image NVARCHAR(1111),
    product_id INT,
    PRIMARY KEY (id)
);

CREATE TABLE [user] (
    id NVARCHAR(255) NOT NULL,
    avatar NVARCHAR(1111),
    email NVARCHAR(1111),
    login_type NVARCHAR(1111),
    password NVARCHAR(1111),
    phone_number NVARCHAR(1111),
    role NVARCHAR(1111),
    user_name NVARCHAR(1111),
    PRIMARY KEY (id)
);

-- Tạo foreign key constraints
ALTER TABLE cart ADD CONSTRAINT FK3d704slv66tw6x5hmbm6p2x3u 
    FOREIGN KEY (product_id) REFERENCES product (id);

ALTER TABLE cart ADD CONSTRAINT FKl70asp4l4w0jmbm1tqyofho4o 
    FOREIGN KEY (user_id) REFERENCES [user] (id);

ALTER TABLE [order] ADD CONSTRAINT FKcpl0mjoeqhxvgeeeq5piwpd3i 
    FOREIGN KEY (user_id) REFERENCES [user] (id);

ALTER TABLE order_item ADD CONSTRAINT FKs234mi6jususbx4b37k44cipy 
    FOREIGN KEY (order_id) REFERENCES [order] (id);

ALTER TABLE order_item ADD CONSTRAINT FK551losx9j75ss5d6bfsqvijna 
    FOREIGN KEY (product_id) REFERENCES product (id);

ALTER TABLE product ADD CONSTRAINT FK1mtsbur82frn64de7balymq9s 
    FOREIGN KEY (category_id) REFERENCES category (id);

ALTER TABLE product_image ADD CONSTRAINT FK6oo0cvcdtb6qmwsga468uuukk 
    FOREIGN KEY (product_id) REFERENCES product (id);
