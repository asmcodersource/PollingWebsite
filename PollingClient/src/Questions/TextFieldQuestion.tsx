import { useState } from 'react';
import { BaseQuestion } from './QuizQuestion'

export interface TextFieldQuestion extends BaseQuestion {
    fieldPlaceholder: string,
}


const TextFieldQuestionComponent = (props: any) => {
    const [questionProps, setQuestionProps] = useState<TextFieldQuestion>(props.question)

    return (
        <div className="text-field-question-wrapper">
            <label className="description">{questionProps.description}</label>
            <input placeholder={questionProps.fieldPlaceholder}></input>
        </div>
    )
}

export default TextFieldQuestionComponent;