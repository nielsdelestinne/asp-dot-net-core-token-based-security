# ASP.NET Core Authentication using JWT

A sample project demonstrating how to provide authentication using ASP.NET Core with JWT (JSON Web Tokens).

## Main concepts
1. Registration (Register as a new user)
2. Authentication (Sign in as an existing user)
3. Authorization (Successfully perform an authorized request by providing a correct (JSON Web) Token)

### 1. Registration
1. A user **registers** himself by providing an e-mail address and a password (the client has the responsibility to not provide this password as just plain-text).
2. The back-end application creates and persists a new `User` object for which it stores the e-mail address and the password (after employing salted password hashing).
	- In other words: the password is never stored as plain-text. It is _salted_, _hashed_ and then persisted in its salted hashed format.
	- The **salt** is always randomly created. Thus, it's important to also persist the used salt.

### 2. Authentication
1. A user **signs in** by providing its e-mail address and password (chosen upon registering).
2. The back-end performs authentication by applying the following steps:
	1. Based on the e-mail (unique constraint), it retrieves the persisted `User` object.
	2. The provided password is salted (by using the salt stored on the `User` object) and hashed. It is then compared to the salted and hashed password that was stored in the found `User` object.
	3. If the salted, hashed passwords don't match, or if no `User` object was found for the provided e-mail address, the user is not authenticated.
	4. If both salted, hashed passwords match, a JSON Web Token (JWT) is created and returned to the user.

### 3. Authorization
1. A user performs a call (request) which is only allowed after authorization (a protected endpoint).
2. Before performing the call, the user adds the `Authorization` request header to the (HTTP) call. As its value, the user provides `Bearer <token>`, 
for which `<token>` is replaced by the JWT received by the back-end after successfully authenticating. 
3. Upon receiving the request, the back-end will inspect the token and validate it. Due to the nature of JSON Web Tokens, the server will be able to validate whether the token is valid or not.
4. Upon receiving a valid token, the back-end will process the request and reply accordingly.
	- If the JWT is invalid, a 403 Forbidden status-code will be returned.

## Sample calls

### 1. Registration
- Call: 
	- POST `https://localhost:44374/api/users/`
- Headers:
	- `Content-Type`: `application/json`
- Body:
	- `{ "Email": "jimmy@gmail.com", "Password": "abc1235" }`

### 2. Authenticate
- Call: 
	- POST `https://localhost:44374/api/users/authenticate`
- Headers:
	- `Content-Type`: `application/json`
- Body:
	- `{ "Email": "jimmy@gmail.com", "Password": "abc1235" }`
- Response Body: The **JWT** will be returned on a successful authentication attempt.

### 3. Perform Authorized Request
- Call: 
	- GET `https://localhost:44374/api/users/current`
- Headers:
	- `Content-Type`: `application/json`
	- `Authorization`: `Bearer <JWT>`
		- Replace `<JWT>` with the received JWT
		- 'Bearer' indicates the Bearer schema.
- Response Body: You'll receive your e-mail address


## Additional information on JWT

JSON Web Tokens enables a secure way to transmit data between two parties in a form of a JSON object. It’s an open standard and it’s a popular mechanism for web authentication.

The JWT (JSON web token) is composed of a **header**, a **payload**, and a **signature** 
which are concatenated by a dot (`.`) and encoded (`header.payload.signature`): `eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJlbWFpbCI6InRpbW15QGdtYWlsLmNvbSIsIm5iZiI6MTU0MjEyNDAxOSwiZXhwIjoxNTQyMTUyODE5LCJpYXQiOjE1NDIxMjQwMTl9.e9myNXvazgagxcrsgZ2k2FUC7gIMEW-sk4nFGIYS79A`

The summary below are excerpts from the following resource: https://medium.com/vandium-software/5-easy-steps-to-understanding-json-web-tokens-jwt-1164c0adfcec. 
The summary should suffice to provide a basic understanding of proper password protection. 

### 1. Header
The header component of the JWT contains information about how the JWT signature should be computed. The header is a JSON object in the following format:
```
{
    "typ": "JWT",
    "alg": "HS256"
}
```

In this JSON, the value of the "typ" key specifies that the object is a JWT, and the value of the "alg" key specifies which hashing algorithm is being used to create the JWT signature component. 
In this example, the HMAC-SHA256 algorithm is used.

The Header is always (Base64) encoded, e.g. `eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9`

### 2. Payload
The payload component of the JWT is the data that's stored inside the JWT. These data are coded in claims, that is statements about an entity, typically the user. 
Claims are often very useful for applying Authorization.
```
{
    "userId": "b08f86af-35da-48f2-8fab-cef3904660bd"
	"email": "jimmy@jimmytimmy.com""
	"membershiplevel": "silver"
}
```

The Payload is always (Base64) encoded, e.g. `eyJlbWFpbCI6InRpbW15QGdtYWlsLmNvbSIsIm5iZiI6MTU0MjEyNDAxOSwiZXhwIjoxNTQyMTUyODE5LCJpYXQiOjE1NDIxMjQwMTl9`

### 3. Signature
The signature gets generated by combining the encoded header and payload together, then hashing the combined result by using a secret key that only server knows.
The signature itself then gets encoded as well, e.g. `e9myNXvazgagxcrsgZ2k2FUC7gIMEW-sk4nFGIYS79A`

### 4. The JSON Web Token (JWT)
Now that we have created all three components, we can create the JWT. Remembering the `header.payload.signature` structure of the JWT, we simply need to combine the components, with periods (.) separating them. We use the base64url encoded versions of the header and of the payload, and the signature we arrived at in step 3.

**JWT**:
```
eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJlbWFpbCI6InRpbW15QGdtYWlsLmNvbSIsIm5iZiI6MTU0MjEyNDAxOSwiZXhwIjoxNTQyMTUyODE5LCJpYXQiOjE1NDIxMjQwMTl9.e9myNXvazgagxcrsgZ2k2FUC7gIMEW-sk4nFGIYS79A
```

### 5. Verifying the JWT

Remember how a JWT contains digital signature that gets generated by combining the header and the payload together. Moreover, its based on a secret key that only server knows.

So, if malicious users try to modify the values in the payload, they have to recreate the signature and for that purpose, 
they need the secret key which the only server has. 

At the server side, we can easily verify a provided JWT by comparing the provided signature with a new signature computed from the header and payload from the JWT provided by the client.

So, we can easily verify the integrity of the JWT by comparing the digital signatures.

### X. JWT Does not protect data
Since JWT's are signed and encoded only, and since JWT's are not encrypted, A JWT does not guarantee any security for sensitive data.

## Additional information on Password Protection

A complete breakdown on password protection by salted password hashing can be found here: https://crackstation.net/hashing-security.htm
The summary below are excerpts from the above resource. The summary should suffice to provide a basic understanding of proper password protection. 

### What is Password Hashing?

Hash algorithms (of which there are many different) are one way functions. They turn any amount of data into a fixed-length "fingerprint" that cannot be reversed. 
They also have the property that if the input changes by even a tiny bit, the resulting hash is completely different (see the example below). 
This is great for protecting passwords, because we want to store passwords in a form that protects them even if the password file (e.g. the database) itself is compromised, 
but at the same time, we need to be able to verify that a user's password is correct. These hash algorithms are made to be secure, not be fast (as the ones in our hash-based data structures)
```
hash("hello") = 2cf24dba5fb0a30e26e83b2ac5b9e29e1b161e5c1fa7425e73043362938b9824
hash("hbllo") = 58756879c05c68dfac9866712fad6a93f8146f337a69afe7dd238f3364946366
hash("waltz") = c0e81794384491161f1777c232bc6bd9ec38f616560b120fda8e90f383853542
```

### Why is Password Hashing not enough?

Due to inventive crackers/hackers, given a hash, the original password can be retrieved by one of the following cracking techniques: 

1. Brute force attacks (Guess the password, hashing each guess, and checking if the guess's hash equals the hash being cracked) 
2. Lookup tables exist (Precomputed hashes and their corresponding plain-text passwords)
3. Rainbow tables exist (Similar to lookup tables but more efficient)

### Adding a Salt

Salting makes it impossible to use lookup tables and rainbow tables to crack a hash.

Lookup tables and rainbow tables only work because each password is hashed the exact same way. 
If two users have the same password, they'll have the same password hashes. 
We can prevent these attacks by randomizing each hash, so that when the same password is hashed twice, the hashes are not the same.

```
hash("hello")                    = 2cf24dba5fb0a30e26e83b2ac5b9e29e1b161e5c1fa7425e73043362938b9824
hash("hello" + "QxLUF1bgIAdeQX") = 9e209040c863f84a31e719795b2577523954739fe5ed3b58a75cff2127075ed1
hash("hello" + "bv5PehSMfV11Cd") = d1d3ec2e6f20fd420d50e2642992841d8338a314b8ea157c9e18477aaef226ab
hash("hello" + "YYLmfY6IehjZMQ") = a49670c3c18b9e079b9cfaf51634f563dc8ae3070db2c4a8544305df1b60f007
```

We can randomize the hashes by appending or prepending a random string, called a salt, to the password before hashing. 
As shown in the example above, this makes the same password hash into a completely different string every time. 
To check if a password is correct, we need the salt, so it is usually stored in the user account database along with the hash, or as part of the hash string itself.

The salt does not need to be secret. Just by randomizing the hashes, lookup tables, reverse lookup tables, and rainbow tables become ineffective. 
An attacker won't know in advance what the salt will be, so they can't pre-compute a lookup table or rainbow table. 
If each user's password is hashed with a different salt, the reverse lookup table attack won't work either.

### Salted Password Hashing

Random long salt + strong hashing algorithm is the right way to securely store passwords.

#### To Store a Password
1. Generate a long random salt.
2. Prepend the salt to the password and hash it with a standard password hashing function.
3. Save both the salt and the hash in the user's database record.

#### To Validate a Password
1. Retrieve the user's salt and hash from the database.
2. Prepend the salt to the given password and hash it using the same hash function.
3. Compare the hash of the given password with the hash from the database. If they match, the password is correct. Otherwise, the password is incorrect.