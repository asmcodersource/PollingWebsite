import QuizQuestion from './QuizQuestion'
import Button from 'react-bootstrap/Button'
import './QuizQuestionAdminWrapper.css'
import React from 'react';

class QuizQuestionAdminWrapper extends React.Component {
    constructor(props: any) {
        super(props);
    }

    render() {
        return (
            <div className="quiz-question-admin-wrapper">
                <label className="text-secondary fst-italic fs-6">#{this.props.question.id} {this.props.question.fieldName}</label>
                <div className="admin-question-wrapper">
                    <QuizQuestion question={this.props.question} />
                </div>
                <div className="buttons-wrapper">
                    <Button onClick={() => this.props.up()}>Up</Button>
                    <Button onClick={() => this.props.down()}>Down</Button>
                    <Button onClick={() => this.props.edit()}>Edit</Button>
                    <Button onClick={() => this.props.remove()}>Remove</Button>
                </div>
            </div>
        )
    }
}

export default QuizQuestionAdminWrapper;