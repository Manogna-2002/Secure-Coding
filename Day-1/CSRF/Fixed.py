from flask import Flask, render_template, request, session, redirect, url_for
from flask_wtf.csrf import CSRFProtect

app = Flask(__name__)
app.secret_key = 'your_secret_key_here'
app.config['WTF_CSRF_ENABLED'] = True  # Enable CSRF protection
csrf = CSRFProtect(app)

# Simulated user database
users = {
    'admin': 'password123'
}

@app.route('/')
def home():
    return "Welcome to the Secure Flask App!"

@app.route('/login', methods=['GET', 'POST'])
def login():
    if request.method == 'POST':
        username = request.form['username']
        password = request.form['password']
        if username in users and users[username] == password:
            session['username'] = username
            return redirect(url_for('dashboard'))
        return "Invalid credentials!"
    return '''
        <form method="post">
            Username: <input type="text" name="username"><br>
            Password: <input type="password" name="password"><br>
            <input type="submit" value="Login">
        </form>
    '''

@app.route('/dashboard')
def dashboard():
    if 'username' in session:
        return f"Welcome {session['username']}! <a href='/logout'>Logout</a>"
    return redirect(url_for('login'))

@app.route('/logout')
def logout():
    session.pop('username', None)
    return redirect(url_for('home'))

@app.route('/change_password', methods=['POST'])
def change_password():
    if 'username' in session:
        new_password = request.form['new_password']
        users[session['username']] = new_password  # Update password
        return "Password changed successfully!"
    return "You are not logged in."

if __name__ == '__main__':
    app.run(debug=True)
