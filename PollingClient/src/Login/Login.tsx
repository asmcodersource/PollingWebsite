import './Login.css'
import { Component, useEffect, useState } from 'react';
import Spinner from 'react-bootstrap/Spinner';
import Button from 'react-bootstrap/Button';
import Modal from 'react-bootstrap/Modal';

class Login extends Component
{
    constructor(props: any) {
        super(props);
        this.state = {
            show: false,
            errorMessage: '',
            isWaiting: false,
        }
        this.handleShow = this.handleShow.bind(this);
        this.setShow = this.setShow.bind(this);
        this.loginHandle = this.loginHandle.bind(this);
    }

    handleShow = function () {
        this.setShow(true);
    };

    setShow = function (newState: boolean) {
        this.setState(prevState => ({
            show: newState
        }));
    };

    loginHandle = async function () {
        this.setState(prevState => ({
            errorMessage: '',
            isWaiting: true,
        }));

        let nickname = document.querySelector('.login-wrapper #nickname');
        let password = document.querySelector('.login-wrapper #password');
        const response = await fetch("api/authorization/authorize", {
            method: "POST",
            headers: {
                "Accept": "application/json",
                "Content-Type": "application/json",
            },
            body: JSON.stringify({
                nickname: nickname.value,
                password: password.value
            })
        });
        if (response.ok === true) {
            let responseJson = await response.json();
            sessionStorage.setItem('token', responseJson['token']);
            this.setState(prevState => ({
                isWaiting: false,
            }));
            this.setShow(false);
            this.props.loggedIn();
        } else {
            let responseBody = await response.json();
            console.log(responseBody);
            if (responseBody.hasOwnProperty('errors')) {
                let errorMessage = ''
                for (const error in responseBody['errors']) {
                    for (const msg of responseBody['errors'][error]) {
                        errorMessage = errorMessage + msg + ". ";
                    }
                }
                this.setState(prevState => ({
                    errorMessage: errorMessage,
                    isWaiting: false,
                }));
            } else {
                this.setState(prevState => ({
                    errorMessage: 'An unknown error has occurred',
                    isWaiting: false,
                }));
            }
        }
    }

    render() {
        return (
            <Modal show={this.state.show} fullscreen={true} onHide={() => this.setShow(false)}>
                <Modal.Header closeButton>
                    <Modal.Title>Login</Modal.Title>
                </Modal.Header>
                <Modal.Body>
                    <div className="center">
                        <div className="login-wrapper">
                            <div>
                                <label>Nickname:</label><br />
                                <input id="nickname" type="text"></input><br />
                                <p className="text-secondary info">Your login nickname must be provided, be between 1 and 256 characters in length, and unique within the system</p>
                            </div>
                        
                            <div>
                                <label>Password:</label><br />
                                <input id="password" type="password"></input><br />
                                <p className="text-secondary info">Your password must be provided, be between 6 and 2048 characters in length, and contain at least one letter and one number.</p>
                            </div>
                            <div className="bottom-wrapper">
                                <div className="error-wrapper">
                                    <p className="text-danger">
                                        {!this.state.isWaiting ?
                                            (this.state.errorMessage)
                                            :
                                            <div className="center">
                                                <Spinner className="waiting-spinner" animation="border" variant="secondary" />
                                            </div>
                                        }
                                    </p>
                                </div>
                                <div className="buttons-wrapper">
                                    <Button onClick={() => this.setShow(false)} variant="outline-secondary">Cancel</Button>
                                    <Button disabled={this.state.isWaiting} onClick={() => this.loginHandle()} variant="primary">Login</Button>
                                </div>
                            </div>
                        </div>
                    </div>
                </Modal.Body>
            </Modal>
        )
    }
}

export default Login;