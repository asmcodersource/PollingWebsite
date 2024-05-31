import { useState } from 'react';
import './QuizQuestion.css';
import TextFieldQuestionComponent from './TextFieldQuestion';
import SelectQuestionComponent from './SelectQuestion';

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

    return (
        <div className="question-wrapper">
            {questionComponent}
        </div>
    );
}

export default QuizQuestion;