import React, { useState } from 'react';
import { BaseQuestion } from './QuizQuestion'

export interface SelectQuestion extends BaseQuestion {
    defaultValue: string,
    options: string[],
}

const SelectQuestionComponent = (props : any) => {
    const [questionProps, setQuestionProps] = useState<SelectQuestion>(props.question);

    return (
        <div className="select-question-wrapper">
            <label className="description">{questionProps.description}</label>
            <select>
                {questionProps.options?.map((option, index) =>
                    <option key={index}>{option}</option>
                )}
            </select>
        </div>
    )
}

export default SelectQuestionComponent;