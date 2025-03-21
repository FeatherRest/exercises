from flask import Flask, jsonify, request
from flask_cors import CORS

app = Flask(__name__)
CORS(app)  # 启用跨域请求支持

current_color = "FFFFFF"


@app.route('/color', methods=['POST'])
def set_color():
    global current_color
    data = request.get_json()

    if data and 'color' in data:
        hex_color = data['color'].lstrip('#')
        if len(hex_color) == 6:
            current_color = hex_color.upper()
            return jsonify({'status': 'success', 'color': current_color}), 200

    return jsonify({'status': 'error', 'message': 'Invalid color format'}), 400


@app.route('/color', methods=['GET'])
def get_color():
    return jsonify({'color': current_color}), 200


if __name__ == '__main__':
    app.run(host='0.0.0.0', port=5000, debug=True)