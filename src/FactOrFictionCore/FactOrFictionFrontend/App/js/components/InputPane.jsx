import React from 'react';
import PropTypes from 'prop-types';
import TextareaAutosize from 'react-autosize-textarea';

import Button from './Button';

import _ from '../../stylesheets/components/_InputPane.scss';
import { VIEW_RESULT } from '../constants/viewTypes';

export default class InputPane extends React.Component {
	static propTypes = {
		changeView: PropTypes.func.isRequired,
		fetchTextEntry: PropTypes.func.isRequired,
	}

	state = {
		textEntry: ""
	}

    render() {
		const { changeView, fetchTextEntry } = this.props;
		const { textEntry } = this.state;
        return (
            <div>
                <div className="left-bar">
					<TextareaAutosize
						className="input-box"
						placeholder="Enter some text..."
						rows={11}
						value={textEntry}
						onChange={e => {
							this.setState({
								textEntry: e.target.value,
							});
						}}
						autoComplete="off"
						autoCorrect="off"
						autoCapitalize="off"
						spellCheck="false"
					/>
				</div>
				<Button
					handleClick={() => {
						fetchTextEntry(textEntry);
						changeView(VIEW_RESULT);
					}}
					content="Start"
				/>
			</div>

        );
	}
}
