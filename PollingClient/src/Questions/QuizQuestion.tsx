import { useState } from 'react';
import './QuizQuestion.css';
import TextFieldQuestionComponent from './TextFieldQuestion';
import SelectQuestionComponent from './SelectQuestion';
import { ValidationErrorResponse, ValidationResult } from './../PollPage/Layout'

export interface BaseQuestion {
    id: number,
    fieldName: string,
    description: string,
    discriminator: string,
    orderRate: number,
};

export interface BaseResponse {
    questionId: number,
}

const QuizQuestion = (props) => {
    const [question, setQuestion] = useState(props.question);

    let questionComponent;

    switch (question.discriminator) {
        case 'TextFieldQuestion':
            questionComponent = <TextFieldQuestionComponent question={question} responseDictionary={props.responseDictionary} />;
            break;
        case 'SelectQuestion':
            questionComponent = <SelectQuestionComponent question={question} responseDictionary={props.responseDictionary} />;
            break;
        default:
            questionComponent = null;
    }

    let errorsMsg = "";
    props.postErrors?.forEach((e: ValidationErrorResponse) => {
        if (e.questionId == props.question.id) {
            e.validationResult.forEach((vr: ValidationResult) => {
                errorsMsg = errorsMsg + vr.errorMessage;
            })
        }
    });

    return (
        <div className="question-wrapper">
            {questionComponent}
            {errorsMsg.length != 0 ?
                (
                <div className="error-box">
                    {errorsMsg}
                </div>
                ) : (<></>)
            }
        </div>
    );
}

export default QuizQuestion;