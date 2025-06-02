# SSL/TLS Certificates

Bu dizin, API ve AuthServer uygulamaları arasındaki güvenli iletişim için gerekli sertifikaları içerir.

## Dosyalar

- `ca.crt`: Sertifika Otoritesi (CA) sertifikası
- `ca.key`: CA özel anahtarı (GİT'e commit edilmemeli!)
- `webapi.pfx`: WebAPI için PKCS#12 formatında sertifika ve özel anahtar (GİT'e commit edilmemeli!)
- `authserver.pfx`: AuthServer için PKCS#12 formatında sertifika ve özel anahtar (GİT'e commit edilmemeli!)

## Güvenlik Uyarısı

Bu dizindeki `.key` ve `.pfx` dosyaları hassas bilgiler içerir ve asla git deposuna commit edilmemelidir. Bu dosyalar .gitignore dosyasına eklenmiştir.

## Yeni Sertifika Oluşturma

Mevcut sertifikaların süresi dolduğunda veya yeni sertifikalara ihtiyaç duyduğunuzda aşağıdaki adımları izleyin:

```bash
# 1. CA oluştur
openssl genrsa -out ca.key 4096
openssl req -x509 -new -nodes -key ca.key -sha256 -days 3650 -out ca.crt -subj "/C=TR/ST=Istanbul/L=Istanbul/O=ECommerce/CN=ECommerce CA"

# 2. WebAPI için sertifika oluştur
openssl genrsa -out webapi.key 2048
openssl req -new -key webapi.key -out webapi.csr -subj "/C=TR/ST=Istanbul/L=Istanbul/O=ECommerce/CN=ecommerce.webapi"

# 3. SAN uzantısı için config dosyası oluştur (webapi.ext)
cat > webapi.ext << EOF
authorityKeyIdentifier=keyid,issuer
basicConstraints=CA:FALSE
keyUsage = digitalSignature, nonRepudiation, keyEncipherment, dataEncipherment
subjectAltName = @alt_names

[alt_names]
DNS.1 = ecommerce.webapi
DNS.2 = localhost
IP.1 = 127.0.0.1
EOF

# 4. Sertifikayı imzala
openssl x509 -req -in webapi.csr -CA ca.crt -CAkey ca.key -CAcreateserial -out webapi.crt -days 825 -sha256 -extfile webapi.ext

# 5. PFX dosyası oluştur
openssl pkcs12 -export -out webapi.pfx -inkey webapi.key -in webapi.crt -certfile ca.crt -passout pass:YourSecurePassword

# 6. AuthServer için sertifika oluştur (benzer adımlar)
openssl genrsa -out authserver.key 2048
openssl req -new -key authserver.key -out authserver.csr -subj "/C=TR/ST=Istanbul/L=Istanbul/O=ECommerce/CN=ecommerce.authserver"

# 7. SAN uzantısı için config dosyası oluştur (authserver.ext)
cat > authserver.ext << EOF
authorityKeyIdentifier=keyid,issuer
basicConstraints=CA:FALSE
keyUsage = digitalSignature, nonRepudiation, keyEncipherment, dataEncipherment
subjectAltName = @alt_names

[alt_names]
DNS.1 = ecommerce.authserver
DNS.2 = localhost
IP.1 = 127.0.0.1
EOF

# 8. Sertifikayı imzala
openssl x509 -req -in authserver.csr -CA ca.crt -CAkey ca.key -CAcreateserial -out authserver.crt -days 825 -sha256 -extfile authserver.ext

# 9. PFX dosyası oluştur
openssl pkcs12 -export -out authserver.pfx -inkey authserver.key -in authserver.crt -certfile ca.crt -passout pass:YourSecurePassword
```

## Docker Compose Yapılandırması

`compose.yaml` dosyasında, oluşturulan sertifikalar Docker containerlarına mount edilir:

```yaml
services:
  ecommerce.webapi:
    # ...
    volumes:
      - ./certs/webapi.pfx:/app/webapi.pfx:ro
      - ./certs/ca.crt:/app/ca.crt:ro

  ecommerce.authserver:
    # ...
    volumes:
      - ./certs/authserver.pfx:/app/auth.pfx:ro
      - ./certs/ca.crt:/app/ca.crt:ro
```
