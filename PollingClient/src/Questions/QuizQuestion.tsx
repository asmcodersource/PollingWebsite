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

const QuizQuestion = (props) => {
    const [question, setQuestion] = useState(props.question);

    let questionComponent;

    switch (question.discriminator) {
        case 'TextFieldQuestion':
            questionComponent = <TextFieldQuestionComponent question={question} />;
            break;
        case 'SelectQuestion':
            questionComponent = <SelectQuestionComponent question={question} />;
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