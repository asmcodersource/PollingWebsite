import './QuizManager.css'
import { Component, useEffect, useState } from 'react';
import Modal from 'react-bootstrap/Modal';
import Tabs from 'react-bootstrap/Tabs';
import Tab from 'react-bootstrap/Tab';
import QuizQuestionsEditor from './QuizQuestionsEditor/QuizQuestionsEditor'
import QuizGeneralSettings from './QuizSettingEditor/QuizGeneralSettings';
import QuizAnswersManager from './QuizAnswersManager/QuizAnswersManager'


class QuizManager extends Component {
    constructor(props: any) {
        super(props);
        this.state = {
            errorMessage: '',
            isWaiting: false,
        };
    }


    render() {
        return (
            <Modal show={this.props.show} fullscreen={true} onHide={this.props.hideDialog}>
                <Modal.Header closeButton>
                    <Modal.Title>{this.props.quiz.title}</Modal.Title>
                </Modal.Header>
                <Modal.Body>
                    <div className="limiter-container-wrapper">
                        <div className="limiter-container">
                            <Tabs
                                variant='pills'
                                defaultActiveKey="home"
                                className="mb-3"
                            >
                                <Tab eventKey="home" title="Settings">
                                    <QuizGeneralSettings quiz={this.props.quiz} />
                                </Tab>
                                <Tab eventKey="questions" title="Questions">
                                    <QuizQuestionsEditor quiz={this.props.quiz} />
                                </Tab>
                                <Tab eventKey="answers" title="Answers">
                                    <QuizAnswersManager quiz={this.props.quiz} />
                                </Tab>
                            </Tabs>
                        </div>
                    </div>
                </Modal.Body>
            </Modal>
        )
    }
}

export default QuizManager;